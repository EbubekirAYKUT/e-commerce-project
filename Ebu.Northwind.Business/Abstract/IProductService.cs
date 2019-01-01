using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebu.Northwind.Business.Abstract
{
    public interface IProductService

    {
        List<Product> GetAll();
        List<Product> GetByCategory(int categorytId);
        void Delete(int productId);
        void Add(Product product);
        void Update(Product product);
        Product GetById(int productId);
    }
}
