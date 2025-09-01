// DTO para Order (sin la referencia circular)
public class OrderDto
{
    public int Id { get; set; }
    public int CustomersId { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public CustomerDto? Customers { get; set; }
}

// DTO para Customer (sin la colección Orders)
public class CustomerDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}

// DTO para crear Order (ya lo tienes)
public class CreateOrderDto
{
    public int CustomersId { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
}