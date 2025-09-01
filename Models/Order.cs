using System;
using System.Collections.Generic;

namespace CRUDconSQL.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomersId { get; set; }

    public string? Status { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual Customer Customers { get; set; } = null!;
}
