using EntryLogManagement.SchoolDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolBLL
{
    internal class StudentService
    {
        private readonly StudentRepository studentRepository;

        public StudentService()
        {
            this.studentRepository = new StudentRepository();
        }


        // Trả về học sinh theo id
        public List<Student> GetStudentID( int id)
        {
           
            return studentRepository.GetStudentId(id);
        }

        // Trả về tất cả học sinh
        public List<Student> GetStudentAll()
        {
            return studentRepository.GetStudentAll();
        }

        // Trả về list học sinh theo class đầu vào
        public List<Student> GetStudentClass(string _class)
        {
            return studentRepository.GetStudentByClass(_class);
        }

        // Thêm học sinh
        public bool AddStudent(Student student)
        {
            return studentRepository.AddStudent(student);
        }

        // Xóa học sinh theo id
        public bool DeleteStudent(int studentid)
        {
            return studentRepository.DeleteStudent(studentid);
        }


        // cập nhật học sinh
        public bool UpdateStudent(Student student , int id)
        {
            return studentRepository.UpdateStudent(student, id);
        }

    }
}
