﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using TestMySql.Models;
using System.Reflection.Metadata;
using EntryLogManagement.SchoolDAL;
using Spectre.Console;

namespace EntryLogManagement.SchoolBLL
{
    internal class SchedulerService
    {
        // Phương thức khởi động bộ lập lịch (scheduler) với các tham số thời gian mặc định và tùy chọn
        public async Task StartScheduler(int hour1 = 12, int minute1 = 0, int hour2 = 21, int minute2 = 55)
        {
            // Tạo một StdSchedulerFactory để quản lý các lịch trình
            StdSchedulerFactory factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler(); // Lấy đối tượng scheduler
            await scheduler.Start(); // Bắt đầu chạy scheduler

            // Job 1: Tạo công việc (job) PhatCanhBaoJob cho nhóm 1
            IJobDetail job1 = JobBuilder.Create<PhatCanhBaoJob>()
                .WithIdentity("PhatCanhBaoJobNhom1", "Nhom1") // Đặt tên công việc và nhóm
                .Build();

            // Tạo lịch trình (trigger) cho công việc 1
            ITrigger trigger1 = TriggerBuilder.Create()
                .WithIdentity("TriggerPhatCanhBaoNhom1", "Nhom1") // Đặt tên lịch trình và nhóm
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour1, minute1)) // Lên lịch hàng ngày tại thời gian cụ thể
                .Build();

            // Job 2: Tạo công việc PhatCanhBaoJob cho nhóm 2
            IJobDetail job2 = JobBuilder.Create<PhatCanhBaoJob>()
                .WithIdentity("PhatCanhBaoJobNhom2", "Nhom2") // Đặt tên công việc và nhóm
                .Build();

            // Tạo lịch trình cho công việc 2
            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("TriggerPhatCanhBaoNhom2", "Nhom2") // Đặt tên lịch trình và nhóm
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour2, minute2)) // Lên lịch hàng ngày tại thời gian cụ thể
                .Build();

            // Lên lịch cho công việc 1 và công việc 2 với các lịch trình tương ứng
            await scheduler.ScheduleJob(job1, trigger1);
            await scheduler.ScheduleJob(job2, trigger2);
        }

        // Định nghĩa công việc phát cảnh báo
        public class PhatCanhBaoJob : IJob
        {
            // Phương thức thực thi công việc
            public async Task Execute(IJobExecutionContext context)
            {
                // Khởi tạo các dịch vụ gửi email và âm thanh và nạp các lớp DAL
                EntryLogRepository entryLogRepository = new EntryLogRepository();
                AlertRepository alertRepository = new AlertRepository();
                MailService mailService = new MailService();
                SoundService soundService = new SoundService();


                var students = entryLogRepository.GetEntryLogToAlert();
                if (students.Count() > 0)
                {
                    foreach (var item in students)
                    {
                        mailService.SendEmail(item.Student.Name);
                    }

                    foreach (var item in students)
                    {
                        alertRepository.InsertAlert(item.StudentId, DateTime.Now);
                    }

                    // Phát âm thanh thông báo
                    soundService.PlaySoundLog();
                }
                else
                {
                    AnsiConsole.Markup("[red]Không có đối tượng trả về.[/]");
                }
                
                
                await Task.CompletedTask;
            }
        }
    }
}
