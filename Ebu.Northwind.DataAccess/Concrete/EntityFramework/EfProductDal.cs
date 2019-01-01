using Ebu.Northwind.DataAccess.Abstract;
using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Ebu.Core.DataAccess.EntityFramework;

namespace Ebu.Northwind.DataAccess.Concrete.EntityFramework
{
    public class EfProductDal:EfEntityRepositoryBase<Product,NorthwindContext>,IProductDal
    {
    }
}
