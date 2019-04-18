using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fame.Service.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductVersionRepository _productVersionRepo;

        public ProductService(IRepositories repositories)
            : base(repositories.Product.Value)
        {
            _productRepo = repositories.Product.Value;
            _productVersionRepo = repositories.ProductVersion.Value;
        }

        public List<string> GetAllDropNames(bool excludeForLayering = false, VersionState? versionState = null)
        {
            return _productVersionRepo.Get()
                .WhereIf(versionState != null, pv => pv.VersionState == VersionState.Active) // We only want to layer/re-layer active dresses but we need to sync all dresses in a group to Spree.
                .WhereIf(excludeForLayering, c => !c.Product.DisableLayering) // Don't return 'DisableLayering' dresses so admin user doesn't accidentally re-layer CCS dresses which will cost $$$$'s
                .Where(pv => pv.Product.DropName != null && pv.Product.DropName != "") // Don't return old products that don't have a DropName becuase they were removed from the product catalogue before adding DropNames
                .Select(c => c.Product.DropName)
                .OrderBy(c => c)
                .Distinct()
                .ToList();
        }

        public List<string> GetActiveProductIdsByDropName(string dropName)
        {
            return _productVersionRepo.Get()
              .Where(pv => pv.VersionState == VersionState.Active)
              .Where(pv => pv.Product.DropName == dropName)
              .Select(c => c.ProductId)
              .ToList();
        }

        public List<string> GetAllProductIdsByDropName(string dropName)
        {
            return _productVersionRepo.Get()
              .Where(pv => pv.Product.DropName == dropName)
              .GroupBy(pv => pv.ProductId)
              .Select(pv => pv.OrderByDescending(p => p.ProductVersionId).FirstOrDefault())
              .Select(pv => pv.ProductId)
              .ToList();
        }

        public List<Product> GetIndexableProducts()
        {
            return _productRepo.Get().Where(p => p.Index).Where(p => p.ProductVersion.Any(pv => pv.VersionState == VersionState.Active)).Include(p => p.Collections).ToList();
        }

        public Product Upsert(Product product)
        {
            var dbProduct = _productRepo.FindById(product.ProductId);

            // Insert if exists
            if (dbProduct == null)
            {
                _productRepo.Insert(product);
                return product;
            }

            // Otherwise Update
            dbProduct.Title = product.Title;
            dbProduct.Index = product.Index;
            dbProduct.ProductType = product.ProductType;
            dbProduct.PreviewType = product.PreviewType;
            dbProduct.Collections = product.Collections;
            dbProduct.DropBoxAssetFolder = product.DropBoxAssetFolder;
            dbProduct.DropName = product.DropName;
            dbProduct.DisableLayering = product.DisableLayering;
            _productRepo.Update(dbProduct);
            return dbProduct;
        }
    }
}