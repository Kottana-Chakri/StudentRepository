using Microsoft.AspNetCore.Mvc;
using StudentRepo.Models;
using StudentRepo.Interfaces;


using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class StudentController : ControllerBase
{
    private readonly IStudentManager _manager;
    public StudentController(IStudentManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    // [Authorize(Roles = "Admin,User")]
    public IActionResult Get()
    {
        return Ok(_manager.GetStudents());
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "Admin")]
    public IActionResult Get(int id)
    {
        var student = _manager.GetStudentById(id);
        if (student == null)
            return NotFound("Student not found");

        return Ok(student);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public IActionResult Post(Student student)
    {
        _manager.AddStudent(student);
        return Ok("Student Added");
    }

    [HttpPut]
    // [Authorize(Roles = "Admin")]
    public IActionResult Put(Student student)
    {
        _manager.UpdateStudent(student);
        return Ok("Student Updated");
    }

    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _manager.DeleteStudent(id);
        return Ok("Student Deleted");
    }
}