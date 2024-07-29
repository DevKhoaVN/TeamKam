using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EntryLogManagement.SchoolBLL
{
    internal class BaseService
    {
        public int GetValidHour(string prompt)
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
        public string GetValidPassword(string prompt)
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
                    AnsiConsole.Markup("[red]Mật khẩu không được để trống. Vui lòng thử lại.[/]");
                }
                else if (!IsPasswordValid(password))
                {
                    AnsiConsole.Markup("[red]Mật khẩu phải chứa ít nhất một ký tự đặc biệt. Vui lòng thử lại.[/]");
                }
                else
                {
                    // Nếu mật khẩu hợp lệ, thoát vòng lặp
                    break;
                }
            }

            return password;
        }

        private bool IsPasswordValid(string password)
        {
            // Kiểm tra sự tồn tại của ít nhất một ký tự đặc biệt
            return password.Any(ch => !char.IsLetterOrDigit(ch));
        }



        public int GetValidMinute(string prompt)
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

        public string GetValidEmail(string prompt)
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


        public int GetValidPhoneNumber(string prompt)
        {
            string input;
            string phoneNumberPattern = @"^\d{10}$"; 

           
            while (true)
            {
               
                AnsiConsole.Markup(prompt);

               
                input = Console.ReadLine();

            
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
                    
                    AnsiConsole.MarkupLine("[red]Số điện thoại của bạn không đúng.[/]");
                }
            }
        }

      

        public int GetIntPrompt(string prompt)
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

        public DateTime GetDate(string prompt)
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
    }
}
