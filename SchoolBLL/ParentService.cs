﻿using EntryLogManagement.SchoolDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolBLL
{
    internal class ParentService
    {
        private readonly AbsentreportRepository absentreportRepository;
        private readonly EntryLogRepository entryLogRepository;

        public ParentService()
        {
            absentreportRepository = new AbsentreportRepository();
            entryLogRepository = new EntryLogRepository();
        }

        public List<Entrylog> ShowLogStudentALL( int id)
        {
            return entryLogRepository.GetEntryLogId(id);
        }

        public List<Entrylog> ShowLogStudentRangeTime(DateTime timeStart , DateTime timeEnd , int id)
        {
            return entryLogRepository.GetEntryLogRangeTimeForParent(timeStart , timeEnd , id);
        }

        public bool SendAbentReport(string Message , int ParentId)
        {
            if(absentreportRepository.InsertAbsentReport(Message , ParentId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Absentreport> ShowAbsentReport( int id)
        {
            return absentreportRepository.GetAbsenreportid(id);
        }

        public List<Absentreport> ShowAbsentReportRangeTime(DateTime timeStart , DateTime timeEnd , int id)
        {
            return absentreportRepository.GetAbsenreportRangeTimeForParent(timeStart, timeEnd, id);
        }



    }
}
