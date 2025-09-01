namespace CRUDconSQL.DTOs
{
    public class CustomerCreateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }

    public class CustomerUpdateDto
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
    }
}
