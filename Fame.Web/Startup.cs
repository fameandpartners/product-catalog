using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.S3;
using Amazon.SQS;
using Autofac;
using Fame.Background;
using Fame.Common;
using Fame.Data.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebApiContrib.Core.Formatter.Csv;
using Fame.Web.Code.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Fame.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });

            var dataProtectionKey = Configuration["FameConfig:DataProtectionKey"];
            if (string.IsNullOrEmpty(dataProtectionKey)) throw new ConfigurationException("DataProtectionKey not configured");
            var path = Path.Join(Environment.CurrentDirectory, "Keys");
            Directory.CreateDirectory(path);
            var dataProtectionKeyBytes = Convert.FromBase64String(dataProtectionKey);
            File.WriteAllBytes(Path.Join(path,"DataProtectionKey.xml"), dataProtectionKeyBytes);
            			
            services.AddLogging(
				builder => {
					builder.AddSerilog(dispose: true);
					//builder.AddFilter("Microsoft", LogLevel.Warning);
					//builder.AddFilter("System", LogLevel.Warning);
				});

            services
                .AddDataProtection()
                .SetApplicationName("product-catalog")
                .DisableAutomaticKeyGeneration()
                .PersistKeysToFileSystem(new DirectoryInfo("./Keys"));

            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{Configuration["Auth0:Domain"]}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = Configuration["Auth0:ClientId"];
                options.ClientSecret = Configuration["Auth0:ClientSecret"];

                // Set response type to code
                options.ResponseType = "code";

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                // Set the callback path, so Auth0 will call back to http://localhost:5000/signin-auth0 
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
                options.CallbackPath = new PathString("/signin-auth0");

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        if (context.Properties.Items.ContainsKey("connection"))
                            context.ProtocolMessage.SetParameter("connection", context.Properties.Items["connection"]);

                        return Task.FromResult(0);
                    },
                    // handle the logout redirection 
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddDbContext(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin());
            });

            services.AddSession(so => so = new SessionOptions() { Cookie = new CookieBuilder() { IsEssential = true } });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                })
                .AddCsvSerializerFormatters()
                .AddSessionStateTempDataProvider();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHttpsRedirection(options => options.HttpsPort = 443);
            services.AddBackgroundServices(Configuration);

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSQS>();
            services.AddAWSService<IAmazonLambda>();

            services.AddDistributedCache(Configuration);

            services.AddOptions();
            services.Configure<FameConfig>(Configuration.GetSection(typeof(FameConfig).Name));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAnyOrigin"));
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Fame_API", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register all Autofac Modules in Assembly
            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
            assemblies.Add(Assembly.GetEntryAssembly());
            builder.RegisterAssemblyModules(assemblies.ToArray());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });

            app.UseSession();  

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect("/account/login");
                }
            });

            //app.UseMiddleware<SerilogMiddleware>();

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute("productionsheet", "Admin", "Admin/ProductionSheet/{pid}", new { action = "Index", controller = "ProductionSheet" });
                routes.MapAreaRoute("admin", "Admin", "Admin/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute("api", "API", "API/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseBackgroundService();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fame_API V1");
            });
        }
    }
}
