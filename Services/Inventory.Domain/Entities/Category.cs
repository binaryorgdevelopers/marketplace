﻿using System.Text.Json.Serialization;
using Inventory.Domain.Abstractions;

namespace Inventory.Domain.Entities;

public class Category : IIdentifiable, ICommon
{
    public Category(string title)
    {
        Title = title;
    }

    public Category()
    {
    }

    public Category(string title, Category? parent)
    {
        Title = title;
        ParentId = parent?.Id;
    }

    public string Title { get; set; }
    public int ProductAmount { get; set; }

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category>? Children { get; set; }

    [JsonIgnore] public IEnumerable<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSession { get; set; }
    public Guid Id { get; set; }
}