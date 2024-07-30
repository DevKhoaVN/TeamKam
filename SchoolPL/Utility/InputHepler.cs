using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolPL.Utility
{
    internal class InputHepler
    {
        public static string PromptUserInput(string promptMessage)
        {
            AnsiConsole.Markup(promptMessage);
            string input = Console.ReadLine();

            return input;
        }

        public static int GetValidHour(string prompt)
        {
            int hour;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out hour) && hour >= 0 && hour <= 23)
                {
                    break;
                }
                AnsiConsole.MarkupLine("[red]Giờ không hợp lệ. Vui lòng nhập giá trị từ 0 đến 23.[/]");
            }
            return hour;
        }

        // Hàm kiểm tra mật khẩu có chứa ít nhất một ký tự đặc biệt
        public static string GetValidPassword(string prompt)
        {
            string password;

            while (true)
            {
                // Hiển thị prompt và nhận đầu vào từ người dùng
                AnsiConsole.Markup(prompt);
                password = Console.ReadLine();

                // Kiểm tra nếu mật khẩu không hợp lệ
                if (string.IsNullOrEmpty(password))
                {
                    AnsiConsole.MarkupLine("[red]Mật khẩu không được để trống. Vui lòng thử lại.[/]");
                }
                else if (!IsPasswordValid(password))
                {
                    AnsiConsole.MarkupLine("[red]Mật khẩu phải chứa ít nhất một ký tự đặc biệt. Vui lòng thử lại.[/]");
                }
                else
                {
                    // Nếu mật khẩu hợp lệ, thoát vòng lặp
                    break;
                }
            }

            return password;
        }

        private static bool IsPasswordValid(string password)
        {
            // Kiểm tra sự tồn tại của ít nhất một ký tự đặc biệt
            return password.Any(ch => !char.IsLetterOrDigit(ch));
        }



        public static int GetValidMinute(string prompt)
        {
            int minute;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out minute) && minute >= 0 && minute <= 59)
                {
                    break;
                }
                AnsiConsole.MarkupLine("[red]Phút không hợp lệ. Vui lòng nhập giá trị từ 0 đến 59.[/]");
            }
            return minute;
        }

        public static string GetValidEmail(string prompt)
        {
            string email;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            while (true)
            {
                AnsiConsole.Markup(prompt);
                email = Console.ReadLine();

                if (Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
                {
                    break;
                }
                AnsiConsole.MarkupLine("[red]Địa chỉ email không hợp lệ! Vui lòng nhập một địa chỉ email hợp lệ.[/]");
            }

            return email;
        }


        public static int GetValidPhoneNumber(string prompt)
        {
            string input;
            // Regular expression pattern for a 10-digit phone number
            string phoneNumberPattern = @"^\d{10}$";

            while (true)
            {
                // Display prompt for user input
                AnsiConsole.Markup(prompt);

                // Read input from the console
                input = Console.ReadLine();

                // Validate input against the phone number pattern
                if (Regex.IsMatch(input, phoneNumberPattern))
                {
                    // Convert valid input to integer
                    if (int.TryParse(input, out int phoneNumber))
                    {
                        return phoneNumber;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Không thể chuyển đổi từ string sang int.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Số điện thoại của bạn không đúng. Vui lòng nhập lại (10 chữ số).[/]");
                }
            }
        }



        public static int GetIntPrompt(string prompt)
        {
            int result = 0;
            bool isValid = false;

            while (!isValid)
            {
                // Hiển thị thông báo và nhận đầu vào dưới dạng chuỗi
                AnsiConsole.Markup(prompt);
                var input = Console.ReadLine();

                // Kiểm tra xem đầu vào có phải là số nguyên không
                isValid = int.TryParse(input, out result);

                // Nếu đầu vào không hợp lệ, hiển thị thông báo lỗi
                if (!isValid)
                {
                    AnsiConsole.MarkupLine("[red]Đầu vào không phải là số nguyên. Vui lòng thử lại.[/]");
                }
            }

            return result;
        }

        public static DateTime GetDate(string prompt)
        {
            string[] formats = new[]
            {
            "dd/MM/yyyy HH:mm",  // Ví dụ: 25/12/2024 14:30
            "MM/dd/yyyy HH:mm",  // Ví dụ: 12/25/2024 14:30
            "yyyy-MM-dd HH:mm",  // Ví dụ: 2024-12-25 14:30
            "dd-MM-yyyy HH:mm",  // Ví dụ: 25-12-2024 14:30
            "yyyy/MM/dd HH:mm",  // Ví dụ: 2024/12/25 14:30
            "dd/MM/yyyy",        // Ví dụ: 25/12/2024
            "MM/dd/yyyy",        // Ví dụ: 12/25/2024
            "yyyy-MM-dd",        // Ví dụ: 2024-12-25
            "dd-MM-yyyy"         // Ví dụ: 25-12-2024
             };

            while (true)
            {
                AnsiConsole.Markup(prompt); // Hiển thị lời nhắc cho người dùng
                string input = Console.ReadLine(); // Đọc dữ liệu đầu vào

                // Thử chuyển đổi đầu vào thành kiểu DateTime với các định dạng khác nhau
                if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date; // Trả về giá trị DateTime nếu chuyển đổi thành công
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Định dạng ngày giờ không hợp lệ. Vui lòng nhập lại theo định dạng sau: dd-MM-yyyy.[/]");
                    AnsiConsole.WriteLine();
                }
            }
        }

        // Nhập phụ huynh và học sinh để thêm
        public static Student GetStudentAndParentInfo()
        {
            // Nhập thông tin phụ huynh
            var parent = new Parent
            {
                ParentName = PromptUserInput("Nhập tên phụ huynh: "),
                ParentEmail = GetValidEmail("Nhập email phụ huynh: "),
                ParentPhone = GetValidPhoneNumber("Nhập số điện thoại phụ huynh: "),
                ParentAddress = PromptUserInput(" Nhập  địa chỉ phụ huynh: ")
            };

            // Nhập thông tin học sinh
            var student = new Student
            {
                Parent = parent,
                Name = PromptUserInput("Nhập tên học sinh: "),
                Gender = PromptUserInput("Nhập giới tính học sinh (Nam/Nữ): "),
                DayOfBirth = GetDate("Nhập ngày sinh học sinh (dd/MM/yyyy): "),
                Class = PromptUserInput("Nhập lớp học sinh: "),
                Address = PromptUserInput("Nhập địa chỉ học sinh: "),
                Phone = GetValidPhoneNumber("Nhập số điện thoại học sinh: "),
                JoinDay = GetDate("Nhập ngày nhập học sinh (dd/MM/yyyy): ")
            };

            return student;
        }
        public static Parent EnterParent()
        {
            Parent parent = new Parent(); // Tạo đối tượng Parent mới

            // Nhập tên phụ huynh, cho phép bỏ trống
            parent.ParentName = AnsiConsole.Ask<string>("Nhập [green]tên phụ huynh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập số điện thoại phụ huynh, cho phép bỏ trống
            var phoneInput = AnsiConsole.Ask<string>("Nhập [green]số điện thoại phụ huynh[/] (nhấn Enter để bỏ qua):");
            parent.ParentPhone = string.IsNullOrWhiteSpace(phoneInput) ? 0 : int.Parse(phoneInput);

            // Nhập email phụ huynh, cho phép bỏ trống
            parent.ParentEmail = AnsiConsole.Ask<string>("Nhập [green]email phụ huynh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập địa chỉ phụ huynh, cho phép bỏ trống
            parent.ParentAddress = AnsiConsole.Ask<string>("Nhập [green]địa chỉ phụ huynh[/] (nhấn Enter để bỏ qua):") ?? null;

            return parent; // Trả về đối tượng Parent đã nhập
        }


        public static Student EnterStudent()
        {
            Student student = new Student(); // Tạo đối tượng Student mới

            student.Parent = EnterParent(); // Nhập thông tin phụ huynh

            // Nhập tên học sinh, cho phép bỏ trống
            student.Name = AnsiConsole.Ask<string>("Nhập [green]tên học sinh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập giới tính học sinh, cho phép bỏ trống
            student.Gender = AnsiConsole.Ask<string>("Nhập [green]giới tính học sinh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập ngày sinh học sinh, cho phép bỏ trống
            var dayOfBirthInput = AnsiConsole.Ask<string>("Nhập [green]ngày sinh học sinh (dd/MM/yyyy)[/] (nhấn Enter để bỏ qua):");
            student.DayOfBirth = string.IsNullOrWhiteSpace(dayOfBirthInput) ? DateTime.MinValue : DateTime.ParseExact(dayOfBirthInput, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Nhập lớp học sinh, cho phép bỏ trống
            student.Class = AnsiConsole.Ask<string>("Nhập [green]lớp học sinh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập địa chỉ học sinh, cho phép bỏ trống
            student.Address = AnsiConsole.Ask<string>("Nhập [green]địa chỉ học sinh[/] (nhấn Enter để bỏ qua):") ?? null;

            // Nhập số điện thoại học sinh, cho phép bỏ trống
            var phoneInput = AnsiConsole.Ask<string>("Nhập [green]số điện thoại học sinh[/] (nhấn Enter để bỏ qua):");
            student.Phone = string.IsNullOrWhiteSpace(phoneInput) ? 0 : int.Parse(phoneInput);

            // Nhập ngày nhập học sinh, cho phép bỏ trống
            var joinDayInput = AnsiConsole.Ask<string>("Nhập [green]ngày nhập học sinh (dd/MM/yyyy)[/] (nhấn Enter để bỏ qua):");
            student.JoinDay = string.IsNullOrWhiteSpace(joinDayInput) ? DateTime.MinValue : DateTime.ParseExact(joinDayInput, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return student; // Trả về đối tượng Student đã nhập
        }




    }
}
