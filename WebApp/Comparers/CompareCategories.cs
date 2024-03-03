using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.EFModels;

namespace WebApp.Comparers
{
    public class CompareCategories : IEqualityComparer<Category>
    {
        public bool Equals(Category cat1, Category cat2)
        {
            if (cat2 == null) return false;

            if (cat1.Id == cat2.Id)
                return true;

            return false;
        }
        public int GetHashCode([DisallowNull] Category obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
