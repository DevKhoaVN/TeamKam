using MySql.Data.MySqlClient;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryLogManagement.SchoolDAL
{
    internal class HandleLogRepository:BaseRposiorty
    {
        public bool HandleRegister(string username, string password, int id)
        {
            try
            {
                using (var connect = GetConnection())
                {
                    connect.Open();
               
                    string query = "INSERT INTO User (UserName, Password, ParentId, RoleId) VALUES (@username, @password, @id, 2)";

                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@id", id);

                        var result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                AnsiConsole.Markup($"[red]Lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();
                return false;
            }
        }

        public int HandleLogin(string username, string password)
        {
            int role = -1; // Giá trị trả về mặc định nếu không tìm thấy người dùng hoặc có lỗi

            try
            {
                using (var connect = GetConnection())
                {
                    // Mở kết nối
                    connect.Open();

                    // Truy vấn kiểm tra tên đăng nhập và mật khẩu
                    string query = @"SELECT RoleId FROM User WHERE UserName = @username AND Password = @password";

                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        // Tạo tham số
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Thực thi truy vấn và lấy dữ liệu
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Đọc giá trị RoleId nếu tìm thấy
                                role = reader.GetInt32("RoleId");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                AnsiConsole.Markup($"[red]Lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }

            return role;
        }
        public bool HandleUserName(string username)
        {
            try
            {
                using (var connect = GetConnection())
                {
                    // Mở kết nối
                    connect.Open();

                    // Truy vấn kiểm tra tên đăng nhập đã tồn tại
                    string query = "SELECT COUNT(*) FROM User WHERE UserName = @username";

                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        // Tạo tham số
                        cmd.Parameters.AddWithValue("@username", username);

                        // Thực thi truy vấn và lấy số lượng bản ghi
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Kiểm tra số lượng bản ghi
                        return count == 0; // Trả về true nếu tên người dùng chưa tồn tại, false nếu đã tồn tại
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                AnsiConsole.Markup($"[red]Lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();
                return false; // Trả về false khi có lỗi xảy ra
            }
        }




    }
}
