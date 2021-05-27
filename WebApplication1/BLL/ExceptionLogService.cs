using System;
using System.Collections.Generic;
using System.Text;
using Base;
using DAL.Repository;
using DAL.Table;
using DAL.TableModel;

namespace BLL
{
    public class ExceptionLogService
    {
        private ExceptionLogRepository exceptionLogRepo;

        public ExceptionLogService(ExceptionLogRepository exceptionLogRepository)
        {
            this.exceptionLogRepo = exceptionLogRepository;
        }

        public string CreateExceptionLog(ExceptionLog exceptionLog)
        {
            return exceptionLogRepo.BatchCreateExceptionLog(new List<ExceptionLog>() { exceptionLog });
        }
    }
}
