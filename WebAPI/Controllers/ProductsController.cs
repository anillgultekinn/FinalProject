using Business.Abstract;
using Business.Concrete;
using Business.Constants;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Loose cpouplin => gevşek bağımlılık
        //naming convention    
        //IoC container -- Inversion of Control
        IProductService _productService;


        public ProductsController(IProductService productService)  //controller IProductService bağımlı    somut bir referansı yok Manager o yüzden IoC kullanacağız
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //productService  erişemiyorum ctor da erişim yok
            //Dependency chain => bağımlılık zinciri

            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product); //clienttan reacttan gönderdiğimiz ürünü buraya koy.
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
