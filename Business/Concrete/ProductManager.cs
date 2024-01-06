using Business.Abstract;
using Business.BusinessAspects;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete;

public class ProductManager : IProductService
{

    IProductDal _productDal;
    ICategoryService _categoryService;

    public ProductManager(IProductDal productDal, ICategoryService categoryService)
    {
        _productDal = productDal;
        _categoryService = categoryService;
    }

    [SecuredOperation("product.add,admin")]
    [ValidationAspect(typeof(ProductValidator))] // add metodunu productvalidator a göre doğrular
    [CacheRemoveAspect("IProductService.Get")]
    public IResult Add(Product product)
    {

        IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
              CheckIfProductCountOfCategoryCount(product.CategoryId), CheckIfCategoryLimitExceded());


        if (result != null) //kurala uymayan bir durum oluşmuşsa
        {
            return result;
        }

        _productDal.Add(product);
        return new SuccessResult(Messages.ProductAdded);

    }


    [CacheAspect]
    public IDataResult<List<Product>> GetAll()
    {
        if (DateTime.Now.Hour == 22)
        {
            return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
        }
        return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);


    }

    public IDataResult<List<Product>> GetAllByCategoryId(int id)
    {
        return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));

    }

    [CacheAspect]
    public IDataResult<Product> GetById(int productId)
    {
        return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));

    }

    public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
    {
        //iki fiyat aralığında olan datayı getirir
        return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
    }

    public IDataResult<List<ProductDetailDto>> GetProductDetails()
    {
        return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
    }

    [ValidationAspect(typeof(ProductValidator))]
    [CacheRemoveAspect("IProductService.Get")] //bellekteki içerisinde Get olan bütün keyleri iptal et dolayısıyla ürünü güncellersek her yerdeki cache sileriz
    public IResult Update(Product product)
    {
        throw new NotImplementedException();
    }

    private IResult CheckIfProductCountOfCategoryCount(int categoryId)
    {
        var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
        if (result > 10)
        {
            return new ErrorResult(Messages.ProductCountOfCategoryError);
        }
        return new SuccessResult();

    }
    private IResult CheckIfProductNameExists(string productName)
    {
        var result = _productDal.GetAll(p => p.ProductName == productName).Any();
        if (result)
        {
            return new ErrorResult(Messages.ProductNameAlreadyExists);
        }
        return new SuccessResult();

    }

    private IResult CheckIfCategoryLimitExceded()
    {
        var result = _categoryService.GetAll();
        if (result.Data.Count > 15)
        {
            return new ErrorResult(Messages.CategoryLimitExceded);
        }
        return new SuccessResult();

    }

    [TransactionScopeAspect]
    public IResult AddTransactionalTest(Product product)
    {
        Add(product);
        if (product.UnitPrice < 10)
        {
            throw new Exception("");
        }

        Add(product);
        return null;
    }
}

