using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Dtos;

namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PersonsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll()
    {
        var items = await _db.Persons
            .OrderBy(p => p.Id)
            .ToListAsync();
        return Ok(items.Select(ToDto));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PersonDto>> GetById(long id)
    {
        var person = await _db.Persons.FindAsync(id);
        return person is null ? NotFound() : Ok(ToDto(person));
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonRequest req)
    {
        var todayTh = TodayInBangkok();
        if (req.BirthDate > todayTh)
        {
            ModelState.AddModelError(nameof(req.BirthDate), "วันเกิดต้องไม่อยู่ในอนาคต");
            return ValidationProblem(ModelState);
        }

        var entity = new Person
        {
            FirstName = req.FirstName.Trim(),
            LastName = req.LastName.Trim(),
            BirthDate = req.BirthDate,
            Address = req.Address?.Trim()
        };
        _db.Persons.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(entity));
    }

    private static PersonDto ToDto(Person p)
    {
        var age = TodayInBangkok().Year - p.BirthDate.Year;
        return new PersonDto(p.Id, p.FirstName, p.LastName, p.BirthDate, age, p.Address);
    }

    private static DateOnly TodayInBangkok()
    {
        var bkk = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");
        var nowTh = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bkk);
        return DateOnly.FromDateTime(nowTh);
    }
}
