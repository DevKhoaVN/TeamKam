using MySql.Data.MySqlClient;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolDAL
{
    internal class AbsentreportRepository: BaseRposiorty
    {
        public AbsentreportRepository() : base()
        {

        }

            // Hàm chèn một bản ghi báo cáo vắng mặt
            public bool InsertAbsentReport(string message, int parentId)
            {
                // Lấy thời gian hiện tại
                DateTime today = DateTime.Now;

                try
                {
                    using (var connect = GetConnection())
                    {
                        // Mở kết nối
                        connect.Open();

                        // Lệnh truy vấn
                        string query = "INSERT INTO AbsentReport (ParentId, CreateDay, Reason) VALUES (@parentId, @today, @message)";

                        // Tạo command
                        using (var cmd = new MySqlCommand(query, connect))
                        {
                            // Thêm tham số
                            cmd.Parameters.AddWithValue("@parentId", parentId);
                            cmd.Parameters.AddWithValue("@today", today);
                            cmd.Parameters.AddWithValue("@message", message);

                            // Thực hiện truy vấn
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Kiểm tra xem có hàng nào bị ảnh hưởng không
                            if (rowsAffected > 0)
                            {
                                return true; // Chèn thành công
                            }
                            else
                            {
                                return false; // Không chèn được
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hiển thị thông báo lỗi
                    AnsiConsole.Markup($"[red]Đã xảy ra lỗi khi thêm báo cáo vắng học.[/]");
                    AnsiConsole.WriteLine();
                    return false; // Chèn thất bại do lỗi
                }
            }

        public List<Absentreport> GetAbsenreportid(int id)
        {
            List<Absentreport> Absentreport = new List<Absentreport>();

            try
            {
                using (var connect = GetConnection())
                {
                    connect.Open();

                    //Lệnh truy vấn
                    string query = "SELECT p.ParentName , s.Name , s.Class , a.Reason , a.CreateDay , p.ParentPhone FROM absentreport as a inner join parent as p on a.ParentId = p.ParentId inner join student as s on s.ParentId = p.ParentId where p.ParentId = @id order by a.CreateDay desc";
                    // Tạo command
                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        // Thực hiện truy vấn
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Đọc dữ liệu truy vấn trả về
                            while (reader.Read())
                            {
                                // Tạo đối tượng Entrylog
                                var absent = new Absentreport
                                {
                                    Parent = new Parent
                                    {
                                        ParentName = reader.GetString("ParentName"),
                                        ParentPhone = reader.GetInt32("ParentPhone"),


                                        Students = new Student
                                        {
                                            Name = reader.GetString("Name"),
                                            Class = reader.GetString("Class")
                                        }
                                    },

                                    CreateDay = reader.GetDateTime("CreateDay"),
                                    Reason = reader.GetString("Reason")

                                };

                                // Thêm đối tượng Alert vào danh sách
                                Absentreport.Add(absent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Đã xảy ra lỗi khi tìm bảng báo cáo vắng học theo ID.[/]");
                AnsiConsole.WriteLine();

            }

            return Absentreport;
        }

        public List<Absentreport> GetAbsenreportAll()
        {
            List<Absentreport> Absentreport = new List<Absentreport>();

            try
            {
                using (var connect = GetConnection())
                {
                    connect.Open();

                    //Lệnh truy vấn
                    string query = "SELECT p.ParentName , s.Name , s.Class , a.Reason , a.CreateDay , p.ParentPhone FROM absentreport as a inner join parent as p on a.ParentId = p.ParentId inner join student as s on s.ParentId = p.ParentId order by a.CreateDay desc";
                    // Tạo command
                    using (var cmd = new MySqlCommand(query, connect))
                    {

                        // Thực hiện truy vấn
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Đọc dữ liệu truy vấn trả về
                            while (reader.Read())
                            {
                                // Tạo đối tượng Entrylog
                                var absent = new Absentreport
                                {
                                    Parent = new Parent
                                    {
                                        ParentName = reader.GetString("ParentName"),
                                        ParentPhone = reader.GetInt32("ParentPhone"),


                                        Students = new Student
                                        {
                                            Name = reader.GetString("Name"),
                                            Class = reader.GetString("Class")
                                        }
                                    },

                                    CreateDay = reader.GetDateTime("CreateDay"),
                                    Reason = reader.GetString("Reason")
                                };

                                // Thêm đối tượng Alert vào danh sách
                                Absentreport.Add(absent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Đã xảy ra lỗi khi tìm  tất cả bảng báo cáo vắng học.[/]");
                AnsiConsole.WriteLine();
            }
            return Absentreport;
        }

        public List<Absentreport> GetAbsenreportRangeTime(DateTime timeStart , DateTime timeEnd)
        {
            List<Absentreport> Absentreport = new List<Absentreport>();

            try
            {
                using (var connect = GetConnection())
                {
                    connect.Open();

                    //Lệnh truy vấn
                    string query = "SELECT p.ParentName , s.Name , s.Class , a.Reason , a.CreateDay , p.ParentPhone FROM absentreport as a inner join parent as p on a.ParentId = p.ParentId inner join student as s on s.ParentId = p.ParentId  where a.CreateDay >= @timeStart && a.CreateDay  <= @timeEnd order by a.CreateDay desc";
                    // Tạo command
                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@timeStart", timeStart);
                        cmd.Parameters.AddWithValue("@timeEnd", timeEnd);
                        // Thực hiện truy vấn
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Đọc dữ liệu truy vấn trả về
                            while (reader.Read())
                            {
                                // Tạo đối tượng Entrylog
                                var absent = new Absentreport
                                {
                                    Parent = new Parent
                                    {
                                        ParentName = reader.GetString("ParentName"),
                                        ParentPhone = reader.GetInt32("ParentPhone"),


                                        Students = new Student
                                        {
                                            Name = reader.GetString("Name"),
                                            Class = reader.GetString("Class")
                                        }
                                    },

                                    CreateDay = reader.GetDateTime("CreateDay"),
                                    Reason = reader.GetString("Reason")

                                };

                                // Thêm đối tượng Alert vào danh sách
                                Absentreport.Add(absent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Đã xảy ra lỗi khi tìm  tất cả bảng báo cáo vắng học theo vùng chọn.[/]");
                AnsiConsole.WriteLine();
            }
            return Absentreport;
        }
        //  Hiển tị theo thời gian cho phụ huynh

        public List<Absentreport> GetAbsenreportRangeTimeForParent(DateTime timeStart, DateTime timeEnd , int id)
        {
            List<Absentreport> Absentreport = new List<Absentreport>();

            try
            {
                using (var connect = GetConnection())
                {
                    connect.Open();

                    //Lệnh truy vấn
                    string query = "SELECT ,p.ParentName , s.Name , s.Class , a.Reason , a.CreateDay , p.ParentPhone FROM absentreport as a inner join parent as p on a.ParentId = p.ParentId inner join student as s on s.ParentId = p.ParentId  where a.CreateDay >= @timeStart and a.CreateDay  <= @timeEnd and p.ParentId = @id order by a.CreateDay desc";
                    // Tạo command
                    using (var cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@timeStart", timeStart);
                        cmd.Parameters.AddWithValue("@timeEnd", timeEnd);
                        cmd.Parameters.AddWithValue(@"id", id);
                        // Thực hiện truy vấn
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Đọc dữ liệu truy vấn trả về
                            while (reader.Read())
                            {
                                // Tạo đối tượng Entrylog
                                var absent = new Absentreport
                                {
                                   
                                    Parent = new Parent
                                    {
                                        ParentName = reader.GetString("ParentName"),
                                        ParentPhone = reader.GetInt32("ParentPhone"),
                                       

                                        Students = new Student
                                        {
                                            Name = reader.GetString("Name"),
                                            Class = reader.GetString("Class")
                                        }
                                    },

                                    CreateDay = reader.GetDateTime("CreateDate"),
                                    Reason = reader.GetString("Reason")

                                };

                                // Thêm đối tượng Alert vào danh sách
                                Absentreport.Add(absent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Đã xảy ra lỗi khi tìm  tất cả bảng báo cáo vắng học theo vùng chọn.[/]");
                AnsiConsole.WriteLine();
            }
            return Absentreport;
        }
    }
}
