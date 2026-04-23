using StudentRepo.Models;

namespace StudentRepo.Interfaces
{
    public interface IStudentManager
    {
        void AddStudent(Student student);
        List<Student> GetStudents();
        Student GetStudentById(int id);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}