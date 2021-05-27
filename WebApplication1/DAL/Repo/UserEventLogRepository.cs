using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Table;

namespace DAL.Repository
{
    public class UserEventLogRepository
    {
        private OneDbContext oneDbContext;

        public UserEventLogRepository(OneDbContext OneDbContext)
        {
            this.oneDbContext = OneDbContext;
        }

        public void SetDbContextForRelation(OneDbContext OneDbContext)
        {
            this.oneDbContext = OneDbContext;
        }

        public IQueryable<UserEventLog> GetUserEventLog(Func<UserEventLog, bool> funcWhere)
        {
            return oneDbContext.UserEventLog.Where(funcWhere).AsQueryable();
        }

        public string BatchCreateUserEventLog(List<UserEventLog> UserEventLogs)
        {
            if (UserEventLogs.Count == 0) return "UserEventLog is null";

            try
            {
                oneDbContext.UserEventLog.AddRange(UserEventLogs);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateUserEventLog(UserEventLog UserEventLog)
        {
            try
            {
                oneDbContext.UserEventLog.Update(UserEventLog);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string DeleteUserEventLog(UserEventLog UserEventLog)
        {
            try
            {
                oneDbContext.UserEventLog.Remove(UserEventLog);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
