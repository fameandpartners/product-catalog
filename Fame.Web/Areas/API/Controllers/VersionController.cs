using System;
using Fame.Service.ChangeTracking;
using Microsoft.AspNetCore.Mvc;

namespace Fame.Web.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        public VersionController(IUnitOfWork unitOfWork)
        { }

        // GET: api/Version
        [HttpGet]
        public IActionResult Get()
        {
            // Ignore transaction in newrelic since we use this as a healthcheck
            NewRelic.Api.Agent.NewRelic.IgnoreTransaction();

            return Ok(new
            {
                BuildNumber = Environment.GetEnvironmentVariable("CIRCLE_BUILD_NUM"),
                BuildBranch = Environment.GetEnvironmentVariable("CIRCLE_BRANCH"),
                BuildSHA = Environment.GetEnvironmentVariable("CIRCLE_SHA1")
            });
        }
    }
}
