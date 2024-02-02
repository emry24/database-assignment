using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService(ProductRepository productRepository, CategoryRepository categoryRepository, ManufactureRepository manufactureRepository, ProductInformationRepository productInformationRepository, ProductPriceRepository productPriceRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;
        private readonly CategoryRepository _categoryRepository = categoryRepository;
        private readonly ManufactureRepository _manufactureRepository = manufactureRepository;
        private readonly ProductInformationRepository _productInformationRepository = productInformationRepository;
        private readonly ProductPriceRepository _productPriceRepository = productPriceRepository;

        public async Task<bool> CreateProductAsync(ProductDto ProductDto)
        {
            try
            {
                if (await _productRepository.ExistingAsync(x => x.ArticleNumber == ProductDto.ArticleNumber))
                {
                    return false;
                }

                var categoryExists = await _categoryRepository.ExistingAsync(x => x.CategoryName == ProductDto.CategoryName);
                int categoryId;

                if (categoryExists)
                {
                    var existsCategoryId = await _categoryRepository.GetAsync(x => x.CategoryName == ProductDto.CategoryName);
                    categoryId = existsCategoryId.Id;
                }
                else
                {
                    var categoryEntity = new Category
                    {
                        CategoryName = ProductDto.CategoryName,
                    };

                    var newCategory = await _categoryRepository.Create(categoryEntity);
                    categoryId = newCategory.Id;
                }

                var manufactureExists = await _manufactureRepository.ExistingAsync(x => x.ManufactureName == ProductDto.ManufactureName);
                int manufactureId;

                if (manufactureExists)
                {
                    var existsManufactureId = await _manufactureRepository.GetAsync(x => x.ManufactureName == ProductDto.ManufactureName);
                    manufactureId = existsManufactureId.Id;
                }
                else
                {
                    var manufactureEntity = new Manufacture
                    {
                        ManufactureName = ProductDto.ManufactureName,
                    };

                    var newManufacture = await _manufactureRepository.Create(manufactureEntity);
                    manufactureId = newManufacture.Id;
                }

                var productEntity = new Product
                {
                    ArticleNumber = ProductDto.ArticleNumber,
                    CategoryId = categoryId,
                    ManufactureId = manufactureId,
                };

                var createdProduct = await _productRepository.Create(productEntity);

                var productInformationEntity = new ProductInformation
                {
                    ArticleNumber = ProductDto.ArticleNumber,
                    ProductTitle = ProductDto.ProductTitle,
                    Ingress = ProductDto.Ingress,
                };

                var createdProductInformation = await _productInformationRepository.Create(productInformationEntity);

                var productPriceEntity = new ProductPrice
                {
                    ArticleNumber = ProductDto.ArticleNumber,
                    Price = ProductDto.Price,
                };

                var createdProductPrice = await _productPriceRepository.Create(productPriceEntity);

                return true;
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }
    }
}
