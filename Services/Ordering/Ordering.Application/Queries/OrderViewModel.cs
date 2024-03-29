﻿using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Queries;

// ReSharper disable  InconsistentNaming
// ReSharper disable  IdentifierTypo
public record OrderViewModel
{
    public string productname { get; init; }
    public int units { get; init; }
    public double unitprice { get; init; }
    public string pictureurl { get; init; }
}

public record Order
{
    public Guid ordernumber { get; init; }
    public DateTime date { get; init; }
    public string status { get; init; }
    public string description { get; init; }
    public string street { get; init; }
    public string city { get; init; }
    public string zipcode { get; init; }
    public string country { get; init; }
    public List<OrderItem> orderitems { get; set; }
    public decimal total { get; set; }
}

public record OrderSummary
{
    public Guid ordernumber { get; init; }
    public DateTime date { get; init; } 
    public string status { get; init; }
    public double total { get; init; }
}

public record CardType
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}