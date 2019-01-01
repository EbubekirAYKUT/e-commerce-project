using Ebu.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebu.Northwind.Business.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetAll();
    }
}
