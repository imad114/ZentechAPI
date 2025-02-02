﻿namespace ZentechAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int MainCategoryID { get; set; }
        public string? MainCategoryName { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }


    }
}
