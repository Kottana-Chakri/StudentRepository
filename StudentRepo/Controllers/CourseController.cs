using Microsoft.AspNetCore.Mvc;
using StudentRepo.models;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly StudentDbContext _context;

    public CourseController(StudentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCourses()
    {
        var courses = _context.Courses
                              .Where(c => c.IsActive)
                              .ToList();

        return Ok(courses);
    }

    [HttpPost]
    public IActionResult AddCourse([FromBody] Courses course)
    {
        if (course == null)
        {
            return BadRequest("Course data is required.");
        }

        _context.Courses.Add(course);
        _context.SaveChanges();

        return Ok(course);
    }
}