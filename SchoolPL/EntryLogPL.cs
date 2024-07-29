using EntryLogManagement.SchoolBLL;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolPL
{
    internal class EntryLogPL
    {
        private readonly BaseService validateService;
        private readonly EntryLogService entryLogService;

        public EntryLogPL()
        {
            validateService = new BaseService();
            entryLogService = new EntryLogService();
        }

        public void ShowEntryLogID()
        {
            int id = validateService.GetIntPrompt("Nhập[green] id bạn muốn tìm kiếm : [/]");
            var log =  entryLogService.GetEtryLogID(id);

            ShowEntrylog_Table(log);
        }
        public void ShowEntryLogAll()
        {

            var log = entryLogService.GetEtryLogAll();

            ShowEntrylog_Table(log);
        }
        public void ShowEntryLogRangeTime()
        {
            DateTime timeStart = validateService.GetDate("Nhập[green] ngày bắt đầu(dd/mm/yyyy): [/]");
            DateTime timeEnd = validateService.GetDate("Nhập[green] ngày kết thúc(dd/mm/yyyy): [/]");

            var log = entryLogService.GetEntryLogRangeTime(timeStart, timeEnd);

            ShowEntrylog_Table(log);
        }

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


    }
}
