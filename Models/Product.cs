﻿using ZentechAPI.Models;

public class Product
{
    public int ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; } 
    public string? UpdatedBy { get; set; }
    // Relation avec la catégorie
    public int CategoryID { get; set; }
    public Category? Category { get; set; } // Propriété de navigation
    // Liste des photos associées
    public List<string> Photos { get; set; }
}
