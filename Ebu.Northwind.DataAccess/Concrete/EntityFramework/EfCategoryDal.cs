using Ebu.Core.DataAccess.EntityFramework;
using Ebu.Northwind.DataAccess.Abstract;
using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebu.Northwind.DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal:EfEntityRepositoryBase<Category,NorthwindContext>,ICategoryDal
    {
    }
}
