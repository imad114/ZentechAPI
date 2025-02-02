﻿using System.ComponentModel.DataAnnotations;

namespace ZentechAPI.Models
{
    public class TechincalDoc
    {
        public int TD_ID { get; set; }
        public string?  TD_CategoryID { get; set; }
        public string? TD_CategoryName { get; set; }

        public string Name { get; set; }
                
        public IFormFile? file { get; set; } 
        public string? filePath { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
