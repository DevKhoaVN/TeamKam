using AForge.Video;
using AForge.Video.DirectShow;
using ZXing.Windows.Compatibility;
using System.Drawing;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TestMySql.Models;
using EntryLogManagement.SchoolDAL;

namespace EntryLogManagement.SchoolBLL
{
    internal class CameraService
    {
        private static FilterInfoCollection videoDevices;
        private static VideoCaptureDevice videoSource;
        private static bool isRunning = true;

        private readonly EntryLogRepository entryLogRepository = new EntryLogRepository();
        private readonly StudentRepository studentRepository = new StudentRepository();
        private readonly SoundService sound = new SoundService();

        // Bật thiết bị camera
        public void TurnOn()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                Console.WriteLine("Không tìm thấy thiết bị video input.");
                return;
            }

            // Sử dụng thiết bị video đầu tiên tìm thấy
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame +=  VideoSource_NewFrame;
            videoSource.Start();

            Console.ReadKey();

            // Dừng thiết bị video
            videoSource.SignalToStop();
            videoSource.WaitForStop();
            
            
        }

        private async void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!isRunning)
                return;

            try
            {
                // Chuyển đổi khung hình từ AForge thành Bitmap
                using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    // Giải mã QR code từ Bitmap
                    var reader = new BarcodeReader();
                    var result = reader.Decode(bitmap);

                    if (result != null && int.TryParse(result.Text, out int StudentID))
                    {
                        // Kiểm tra sinh viên có ID tương ứng có tồn tại không
                        List<Student> student = studentRepository.GetStudentId(StudentID);

                        if (student.FirstOrDefault() != null)
                        {
                            var status = "Int"; // Trạng thái mặc định nếu không tìm thấy bản ghi trước đó

                            // Lấy bản ghi ra vào mới nhất của sinh viên
                            List<Entrylog> entry = entryLogRepository.GetEntryLogId(StudentID);

                            // Kiểm tra xem có bản ghi gần đây trong vòng 5 giây không
                            if (entry != null && entry.FirstOrDefault() != null && (DateTime.Now - entry.FirstOrDefault().LogTime).TotalSeconds < 5)
                            {
                                return;
                            }

                            // Xác định trạng thái log dựa trên bản ghi trước đó
                            if (entry != null && entry.FirstOrDefault() != null && entry.FirstOrDefault().Status == "Int")
                            {
                                status = "Out";
                            }
                            else
                            {
                                status = "Int";
                            }

                            var time = DateTime.Now;
                            // Tạo bản ghi ra vào mới
                            Entrylog log = new Entrylog
                            {
                                LogTime = time,
                                StudentId = StudentID,
                                Status = status
                            };

                            // Chèn bản ghi vào cơ sở dữ liệu
                             await entryLogRepository.InsertEntryLogAsync(log);

                            // Hiển thị bảng
                            await RenderTableAsync(1 , 15);

                            // Phát âm thanh
                            sound.PlaySoundCamera();

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi giải mã QR code: {ex.Message}");
                // Implement logging or further error handling here
            }
        }

        // Hiển thị bảng ra vào
        private async Task RenderTableAsync(int pageNumber = 1, int pageSize = 15)
        {
            // Check if the service is running; if not, complete the task immediately
           

            // Lấy danh sách bảng ra vào
            var logs = await entryLogRepository.GetEntryLogTodayAsync();

            // Xác định số lượng trang
            int totalRecords = logs.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Kiểm tra số trang hợp lệ
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages) pageNumber = totalPages;

            // Phân trang dữ liệu
            var pagedLogs = logs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Định nghĩa bảng và thêm cột
            var table = new Table().Expand();
            table.Title($"[yellow]Bảng ra vào - Trang {pageNumber}/{totalPages}[/]");
            table.AddColumn("ID");
            table.AddColumn("Học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian");
            table.AddColumn("Trạng thái");

            // Thêm dữ liệu vào hàng
            foreach (var item in pagedLogs)
            {
                table.AddRow(
                    $"{item.StudentId}",
                    $"{item.Student?.Name ?? "N/A"}",
                    $"{item.Student?.Class ?? "N/A"}",
                    $"{item.LogTime}",
                    $"{item.Status}"
                );
            }

            // Hiển thị
            AnsiConsole.Clear();
            AnsiConsole.Render(table);

            if (!isRunning)
            {
                await Task.CompletedTask;
                return;
            }
        }

    }
}
