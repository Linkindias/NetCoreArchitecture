using System;
using System.Collections.Generic;
using System.Text;
using Base;
using DAL.Repository;
using DAL.Table;

namespace BLL
{
    public class OperateLogService
    {
        private UserEventLogRepository userEventLogRepo;

        public OperateLogService(UserEventLogRepository userEventLogRepository)
        {
            this.userEventLogRepo = userEventLogRepository;
        }

        public string CreateOperateLog(UserEventLog userEventLog)
        {
            return userEventLogRepo.BatchCreateUserEventLog(new List<UserEventLog>() { userEventLog });
        }
    }
}
