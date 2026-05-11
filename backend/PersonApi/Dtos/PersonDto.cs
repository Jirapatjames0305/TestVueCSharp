namespace PersonApi.Dtos;

public record PersonDto(
    long Id,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    int Age,
    string? Address
);
