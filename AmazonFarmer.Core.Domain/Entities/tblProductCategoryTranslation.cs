﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblProductCategoryTranslation
    {
        [Key]
        public int ID { get; set; }
        public int ProductCategoryID { get; set; }
        public string LanguageCode { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        [ForeignKey("ProductCategoryID")]
        public virtual tblProductCategory ProductCategory { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; }
    }
}
