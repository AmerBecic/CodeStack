﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.EFModels;

namespace WebApp.Models
{
    public class CategoriesToUserModel
    {
        public string UserId { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Category> CategoriesSelected { get; set; }
    }
}
