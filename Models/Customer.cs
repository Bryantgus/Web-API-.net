using System;
using System.Collections.Generic;

namespace CRUDconSQL.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
