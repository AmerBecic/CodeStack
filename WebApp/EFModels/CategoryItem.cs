﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.EFModels
{
    public class CategoryItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        public int CategoryId { get; set; }

        public int MediaTypeId { get; set; }

        public DateTime DateItemReleased { get; set; }



    }
}
