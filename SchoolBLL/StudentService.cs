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


        public List<Student> GetStudentID( int id)
        {
           
            return studentRepository.GetStudentId(id);
        }
        public List<Student> GetStudentAll()
        {
            return studentRepository.GetStudentAll();
        }

        public List<Student> GetStudentClass(string _class)
        {
            return studentRepository.GetStudentByClass(_class);
        }

    }
}
