namespace PersonApi.Data;

public class Person
{
    public long Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}
