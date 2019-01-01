using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.DataAccess.Abstract;
using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;


namespace Ebu.Northwind.Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public void Add(Product product)
        {
            _productDal.Add(product);
        }

        public void Delete(int productId)
        {
            _productDal.Delete(new Product { ProductId=productId});
        }

        public List<Product> GetAll()
        {
           return _productDal.GetList();
        }

        public List<Product> GetByCategory(int categorytId)
        {
           return _productDal.GetList(p => p.CategoryId == categorytId || categorytId==0);
        }

        public void Update(Product product)
        {
            _productDal.Update(product);
        }
        public Product GetById(int ProductId)
        {
           return _productDal.Get(c => c.ProductId == ProductId);
        }
    }
}
