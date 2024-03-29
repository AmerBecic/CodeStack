﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.EFModels
{
    public class CategoryItem
    {
        private DateTime _releaseDate = DateTime.MinValue;

        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Please select an item from the {0} dropdown list")]
        [Display(Name ="Media Type")]
        public int MediaTypeId { get; set; }

        [NotMapped]
        public virtual ICollection<SelectListItem> MediaTypes { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Release Date")]
        public DateTime DateItemReleased
        {
            get
            {
                return (_releaseDate == DateTime.MinValue) ? DateTime.Now : _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }
        }

        [NotMapped] //Migrations ignore this
        public int ContentId { get; set; }

    }
}
