using EntryLogManagement.SchoolBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TestMySql.Models;

namespace EntryLogManagement.SchoolPL
{
    internal class StudentPL
    {

        private readonly BaseService validateServices;
        private readonly StudentService studentService;

        public StudentPL()
        {
            validateServices = new BaseService();
            studentService = new StudentService();
        }
        public void ShowStudentInforÌD()
        {
            int id = validateServices.GetIntPrompt("NHập[green] id bạn muốn tìm:[/]");

            var students = studentService.GetStudentID(id);

            StudentInfor_Table(students);

        }

        public void ShowStudentInforAll()
        {
            var student = studentService.GetStudentAll();

            StudentInfor_Table(student);
        }

        public void ShowStudentInforClass()
        {
            string lop = Console.ReadLine();
            var students = studentService.GetStudentClass(lop);

            StudentInfor_Table(students);
        }
        public void StudentInfor_Table(List<Student> studentInfor)
        {
            int pageSize = 15;
            int totalPages = (int)Math.Ceiling(studentInfor.Count / (double)pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear(); // Xóa màn hình trước khi hiển thị trang mới
                Console.WriteLine($"Trang {currentPage} / {totalPages}");

                // Tạo bảng và thêm các cột
                var table = new Table();
                table.Title("[red]Danh sách thông tin học sinh[/]").HeavyEdgeBorder();
                table.AddColumn("ID học sinh");
                table.AddColumn("Tên học sinh");
                table.AddColumn("Giới tính");
                table.AddColumn("Ngày sinh");
                table.AddColumn("Lớp");
                table.AddColumn("Địa chỉ");
                table.AddColumn("Số điện thoại");
                table.AddColumn("Tên phụ huynh");
                table.AddColumn("Email phụ huynh");
                table.AddColumn("Số điện thoại phụ huynh");
                table.AddColumn("Địa chỉ phụ huynh");

                // Tính toán các dòng cần hiển thị trên trang hiện tại
                var pageData = studentInfor.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Thêm dữ liệu vào hàng
                foreach (var student in pageData)
                {
                    table.AddRow(
                        $"{student.StudentId}",
                        $"{student.Name}",
                        $"{student.Gender}",
                        $"{student.DayOfBirth:yyyy-MM-dd}",
                        $"{student.Class}",
                        $"{student.Address}",
                        $"{student.Phone}",
                        $"{student.Parent.ParentName}",
                        $"{student.Parent.ParentEmail}",
                        $"{student.Parent.ParentPhone}",
                        $"{student.Parent.ParentAddress}"
                    );
                }

                // Hiển thị bảng
                AnsiConsole.Render(table);
                AnsiConsole.WriteLine();

                // Điều hướng người dùng
                if (currentPage < totalPages)
                {
                    Console.WriteLine("Nhấn [Enter] để xem trang tiếp theo hoặc [Esc] để thoát.");
                }
                else
                {
                    Console.WriteLine("Nhấn [Enter] để thoát.");
                }

                // Nhận đầu vào từ người dùng để điều hướng
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    if (currentPage < totalPages)
                    {
                        currentPage++;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

    }
}
