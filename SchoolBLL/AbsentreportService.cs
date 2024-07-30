using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntryLogManagement.SchoolDAL;
using TestMySql.Models;

namespace EntryLogManagement.SchoolBLL
{
    internal class AbsentreportService
    {
        private readonly AbsentreportRepository absentreportRepository;

        public AbsentreportService()
        {
            absentreportRepository = new AbsentreportRepository();
        }

        public List<Absentreport> GetReportID(int id)
        {
            var  ab = absentreportRepository.GetAbsenreportid(id);
           if (ab.Count <= 0)
            {
                Console.WriteLine("Loi");
            }
            return ab;
        }

        public List<Absentreport> GetReportAll()
        {
            return absentreportRepository.GetAbsenreportAll();
        }

        public List<Absentreport> GetReportRangeTime(DateTime timeStart, DateTime timeEnd)
        {
            return absentreportRepository.GetAbsenreportRangeTime(timeStart, timeEnd);
        }

        public List<Absentreport> GetReportRangeTimeForParent(DateTime timeStart, DateTime timeEnd , int id)
        {
            return absentreportRepository.GetAbsenreportRangeTimeForParent(timeStart, timeEnd , id);
        }

    }
}
