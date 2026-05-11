using System.ComponentModel.DataAnnotations;

namespace PersonApi.Dtos;

public record CreatePersonRequest(
    [Required, StringLength(100, MinimumLength = 1)] string FirstName,
    [Required, StringLength(100, MinimumLength = 1)] string LastName,
    [Required] DateOnly BirthDate,
    [StringLength(500)] string? Address
);
