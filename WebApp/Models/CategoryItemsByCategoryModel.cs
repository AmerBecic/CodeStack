using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class CategoryItemsByCategoryModel
    {
        public int CategoryId { get; set; }

        public string Title { get; set; }

        public IGrouping<int, CategoryItemDetailsModel> CategoryItems { get; set; }
    }
}
