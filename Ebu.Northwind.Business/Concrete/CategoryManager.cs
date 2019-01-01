using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.DataAccess.Abstract;
using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebu.Northwind.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public List<Category> GetAll()
        {
            return _categoryDal.GetList();
        }
    }
}
