using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.EFModels;

namespace WebApp.Models
{
    public class CategoryDetailsModel
    {
        public IEnumerable<CategoryItemsByCategoryModel> CategoryItemsByCategoryModels { get; set; }
        public IEnumerable<Category> Categories { get; set;}


        }
}
