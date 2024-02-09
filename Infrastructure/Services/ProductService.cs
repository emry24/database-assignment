using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
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

                    var newCategory = await _categoryRepository.CreateAsync(categoryEntity);
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

                    var newManufacture = await _manufactureRepository.CreateAsync(manufactureEntity);
                    manufactureId = newManufacture.Id;
                }


                var productEntity = new Product
                {
                    ArticleNumber = productDto.ArticleNumber,
                    CategoryId = categoryId,
                    ManufactureId = manufactureId,
                };

                var createdProduct = await _productRepository.CreateAsync(productEntity);


                var productInformationEntity = new ProductInformation
                {
                    ArticleNumber = productDto.ArticleNumber,
                    ProductTitle = productDto.ProductTitle,
                    Ingress = productDto.Ingress,
                    Description = productDto.Description,
                    Specification = productDto.Specification,
                };

                var createdProductInformation = await _productInformationRepository.CreateAsync(productInformationEntity);


                var productPriceEntity = new ProductPrice
                {
                    ArticleNumber = productDto.ArticleNumber,
                    Price = productDto.Price,
                };

                var createdProductPrice = await _productPriceRepository.CreateAsync(productPriceEntity);

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
                        CategoryName = productEntity.Category.CategoryName,
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

        public async Task<bool> UpdateProductAsync(string articleNumber, ProductDto updatedProductDto)
        {
            try
            {
                var product = await _productRepository.GetAsync(x => x.ArticleNumber == articleNumber);
                if (product != null)
                {
                    var productInfoEntity = await _productInformationRepository.GetAsync(x => x.ArticleNumber == product.ArticleNumber);
                    productInfoEntity.ProductTitle = updatedProductDto.ProductTitle;
                    productInfoEntity.Ingress = updatedProductDto.Ingress;
                    productInfoEntity.Description = updatedProductDto?.Description;
                    productInfoEntity.Specification = updatedProductDto?.Specification;

                    var updatedProductInfo = await _productInformationRepository.UpdateAsync(x => x.ArticleNumber == productInfoEntity.ArticleNumber, productInfoEntity);

                    var existingManufacture = await _manufactureRepository.GetAsync(c => c.ManufactureName == updatedProductDto!.ManufactureName);
                    if (existingManufacture != null)
                    {
                        product.ManufactureId = existingManufacture.Id;
                    }
                    else
                    {
                        var newManufacture = new Manufacture { ManufactureName = updatedProductDto!.ManufactureName };
                        newManufacture = await _manufactureRepository.CreateAsync(newManufacture);
                        product.ManufactureId = newManufacture.Id;
                    }

                    var updatedManufacture = await _productRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, product);

                    var existingCategory = await _categoryRepository.GetAsync(c => c.CategoryName == updatedProductDto!.CategoryName);
                    if (existingCategory != null)
                    {
                        product.CategoryId = existingCategory.Id;
                    }
                    else
                    {
                        var newCategory = new Category { CategoryName = updatedProductDto!.CategoryName };
                        newCategory = await _categoryRepository.CreateAsync(newCategory);
                        product.CategoryId = newCategory.Id;
                    }

                    var updatedCategory = await _productRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, product);

                    var productPriceEntity = await _productPriceRepository.GetAsync(x => x.ArticleNumber == articleNumber);
                    productPriceEntity.Price = updatedProductDto!.Price;

                    var updatedPrice = await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == productPriceEntity.ArticleNumber, productPriceEntity);

                    return updatedProductInfo != null && updatedManufacture != null && updatedCategory != null && updatedPrice != null; 
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }


        public async Task<bool> DeleteProductByArticleNrAsync(string articleNumber)
        {
            try
            {
                var product = await _productRepository.GetAsync(x => x.ArticleNumber == articleNumber);
                if (product != null)
                {
                    await _productRepository.DeleteAsync(x => x.ArticleNumber == articleNumber);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }
    }
}
