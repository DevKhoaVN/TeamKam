﻿using EntryLogManagement.SchoolBLL;
using EntryLogManagement.SchoolPL.Utility;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolPL
{
    internal class ParentPL
    {
        private readonly ParentService parentService;
        private readonly AbsentreportService absentreportService;
        

        public ParentPL()
        {
            parentService = new ParentService();
            absentreportService = new AbsentreportService();
        }

        public void ShowLogStudentAll(int id)
        {
             var logforparent = parentService.ShowLogStudentALL(id);
            ShowEntrylog_Table(logforparent);
        }

        public void ShowLogStudentRangeTime(int id )
        {
            DateTime timeStart = InputHepler.GetDate("Nhập[green] ngày bắt đầu(dd/mm/yyyy): [/]");
            DateTime timeEnd = InputHepler.GetDate("Nhập[green] ngày kết thúc(dd/mm/yyyy): [/]");

            var logforparent = parentService.ShowLogStudentRangeTime(timeStart, timeEnd, id);

            ShowEntrylog_Table(logforparent);
        }


        public void SendReport(int id)
        {
            var message = InputHepler.PromptUserInput("Nhập[green] lí do vắng học: [/]");

           var result =  parentService.SendAbentReport(message, id);

            if(result)
            {
                AnsiConsole.Markup("[green]Gửi báo cáo vắng học thành công.[/]");
            }
            else
            {
                AnsiConsole.Markup("[green]Gửi báo cáo vắng học thất bại.[/]");
            }
        }
        public void ShowAbsentStudentAll(int id )
        {
            var absentforparent = absentreportService.GetReportID(id);

            ShowAbsentReport_Table(absentforparent);
        }


        /*public void ShowAbsentStudentRangeTime(int id)
        {
            DateTime timeStart = validateService.GetDate("Nhập[green] ngày bắt đầu(dd/mm/yyyy): [/]");
            DateTime timeEnd = validateService.GetDate("Nhập[green] ngày kết thúc(dd/mm/yyyy): [/]");

            var absentforparent = absentreportService.GetReportRangeTimeForParent(timeStart, timeEnd, id);

            ShowAbsentReport_Table(absentforparent);
        }*/

        // show bảng entrylog
        public void ShowEntrylog_Table(List<Entrylog> entryLogs)
        {
            int pageSize = 15;
            int totalRecords = entryLogs.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear(); // Xóa màn hình trước khi hiển thị trang mới
                Console.WriteLine($"Trang {currentPage} / {totalPages}");

                // Tạo bảng và thêm các cột
                var table = new Table();
                table.Title($"[#ffff00]Danh sách học sinh ra vào[/]").HeavyEdgeBorder();
                table.AddColumn("ID học sinh");
                table.AddColumn("Tên học sinh");
                table.AddColumn("Lớp");
                table.AddColumn("Thời gian bản ghi");
                table.AddColumn("Trạng thái");

                // Tính toán các dòng cần hiển thị trên trang hiện tại
                var pageData = entryLogs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Thêm các hàng vào bảng
                foreach (var log in pageData)
                {
                    table.AddRow(
                        $"{log.StudentId}",
                        $"{log.Student.Name}",
                        $"{log.Student.Class}",
                        $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                        $"{log.Status}"
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

        // Show bảng absentreport
        public void ShowAbsentReport_Table(List<Absentreport> data)
        {
            int pageSize = 15;
            int totalRecords = data.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear(); // Xóa màn hình trước khi hiển thị trang mới
                Console.WriteLine($"Trang {currentPage} / {totalPages}");

                // Tạo bảng và thêm các cột
                var table = new Table().Expand();
                table.Title($"[#ffff00]Bảng báo cáo vắng học[/]");
                table.AddColumn("ID học sinh");
                table.AddColumn("Tên học sinh");
                table.AddColumn("Tên phụ huynh");
                table.AddColumn("Lớp");
                table.AddColumn("Ngày báo cáo");
                table.AddColumn("Lý do");

                // Tính toán các dòng cần hiển thị trên trang hiện tại
                var pageData = data.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Thêm các hàng vào bảng
                foreach (var report in pageData)
                {
                    table.AddRow(
                        $"{report.Parent.Students.Parent}",
                        $"{report.Parent.ParentName}",
                        $"{report.Parent.Students.Class}",
                        $"{report.CreateDay:yyyy-MM-dd}",
                        $"{report.Reason}"
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
