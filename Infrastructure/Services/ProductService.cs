using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProductService(ProductRepository productRepository, CategoryRepository categoryRepository, ManufactureRepository manufactureRepository, ProductInformationRepository productInformationRepository, ProductPriceRepository productPriceRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;
        private readonly CategoryRepository _categoryRepository = categoryRepository;
        private readonly ManufactureRepository _manufactureRepository = manufactureRepository;
        private readonly ProductInformationRepository _productInformationRepository = productInformationRepository;
        private readonly ProductPriceRepository _productPriceRepository = productPriceRepository;

        public async Task<bool> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                if (await _productRepository.ExistingAsync(x => x.ArticleNumber == productDto.ArticleNumber))
                {
                    return false;
                }

                var categoryExists = await _categoryRepository.ExistingAsync(x => x.CategoryName == productDto.CategoryName);
                int categoryId;

                if (categoryExists)
                {
                    var existsCategoryId = await _categoryRepository.GetAsync(x => x.CategoryName == productDto.CategoryName);
                    categoryId = existsCategoryId.Id;
                }
                else
                {
                    var categoryEntity = new Category
                    {
                        CategoryName = productDto.CategoryName,
                    };

                    var newCategory = await _categoryRepository.Create(categoryEntity);
                    categoryId = newCategory.Id;
                }


                var manufactureExists = await _manufactureRepository.ExistingAsync(x => x.ManufactureName == productDto.ManufactureName);
                int manufactureId;

                if (manufactureExists)
                {
                    var existsManufactureId = await _manufactureRepository.GetAsync(x => x.ManufactureName == productDto.ManufactureName);
                    manufactureId = existsManufactureId.Id;
                }
                else
                {
                    var manufactureEntity = new Manufacture
                    {
                        ManufactureName = productDto.ManufactureName,
                    };

                    var newManufacture = await _manufactureRepository.Create(manufactureEntity);
                    manufactureId = newManufacture.Id;
                }


                var productEntity = new Product
                {
                    ArticleNumber = productDto.ArticleNumber,
                    CategoryId = categoryId,
                    ManufactureId = manufactureId,
                };

                var createdProduct = await _productRepository.Create(productEntity);


                var productInformationEntity = new ProductInformation
                {
                    ArticleNumber = productDto.ArticleNumber,
                    ProductTitle = productDto.ProductTitle,
                    Ingress = productDto.Ingress,
                    Description = productDto.Description,
                    Specification = productDto.Specification,
                };

                var createdProductInformation = await _productInformationRepository.Create(productInformationEntity);


                var productPriceEntity = new ProductPrice
                {
                    ArticleNumber = productDto.ArticleNumber,
                    Price = productDto.Price,
                };

                var createdProductPrice = await _productPriceRepository.Create(productPriceEntity);

                return true;
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var productEntities = await _productRepository.GetAllAsync();

                if (productEntities != null)
                {
                    var productDtos = productEntities.Select(productEntity => new ProductDto
                    {
                        ProductTitle = productEntity.ProductInformation!.ProductTitle,
                        ArticleNumber = productEntity.ArticleNumber,
                        ManufactureName = productEntity.Manufacture.ManufactureName,
                    });

                    return productDtos.ToList();
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }


        public async Task<ProductDto> GetProductByArticleNrAsync(string articleNumber)
        {
            try
            {
                var product = await _productRepository.GetAsync(x => x.ArticleNumber == articleNumber);
                if (product != null)
                {
                    var productDto = new ProductDto
                    {
                        ProductTitle = product.ProductInformation!.ProductTitle,
                        ArticleNumber= product.ArticleNumber,
                        ManufactureName= product.Manufacture.ManufactureName,
                        Ingress = product.ProductInformation.Ingress,
                        Description = product.ProductInformation.Description, 
                        Price = product.ProductPrice!.Price,
                        Specification = product.ProductInformation.Specification,
                        CategoryName = product.Category.CategoryName,
                    };

                    return productDto;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }
    }
}
