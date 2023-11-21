using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        //proje başlayınca bellekte bir tane product listesi oluştur
        List<Product> _products;

        //Oracle,Sql Server, Postgres , MongoDb'den geliyormuş gibi simüle ediyoruz
        public InMemoryProductDal()
        {
            _products = new List<Product> {
                new Product{ProductId=1, CategoryId=1, ProductName ="Bardak", UnitPrice=15, UnitsInStock=15},
                new Product{ProductId=2, CategoryId=1, ProductName ="Kamera", UnitPrice=500, UnitsInStock=3},
                new Product{ProductId=3, CategoryId=2, ProductName ="Telefon", UnitPrice=1500, UnitsInStock=2},
                new Product{ProductId=4, CategoryId=2, ProductName ="Klavye", UnitPrice=150, UnitsInStock=65},
                new Product{ProductId=5, CategoryId=2, ProductName ="Fare", UnitPrice=85, UnitsInStock=1}

            };
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            //_products.Remove(product); bu kod ile listeden elemanları silemeyiz çünkü yukarıdaki _products ile aynı referans numarasına sahip değil
            //arayüzden bir tane ürün newleyip gönderdiğinde id si aynı olsa bile referansı farklı olduğu için ürünler listesinden silemiyor.
            //List<string> veya List <int> gönderseyedik silerdi çünkü değer tipli

            //LINQ liste bazlı yapılarıı sql gibi filtrelebiliyoruz where komutunu kullanarak
            //Product productToDelete=null;
            //foreach (var p in _products)
            //{
            //    if (product.ProductId==p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}
            //(p =>); tek tek dolaşır (foreach gibi)
            //genelde id olan aramalarda SingleOrDefault kullanırız
            //ürünü silerken primarykey kullanırız.
            Product productToDelete = _products.SingleOrDefault(p => p.ProductId == product.ProductId);


            _products.Remove(productToDelete);

        }

        public List<Product> GetAll()
        {
            return _products;
        }



        public void Update(Product product)
        {
            //Gönderdiğim ürün id'sine sahip olan(Product product) listedeki ürünü bul
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;


        }
        public List<Product> GetAllByCategory(int categoryId)
        {
            //where koşulu içindeki şarta uyan bütün elemanları yeni bir liste haline getirir ve onu döndürür.
            return _products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
