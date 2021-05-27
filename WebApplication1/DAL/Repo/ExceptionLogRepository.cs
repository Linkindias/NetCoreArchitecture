using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Table;
using DAL.TableModel;

namespace DAL.Repository
{
    public class ExceptionLogRepository
    {
        private OneDbContext oneDbContext;

        public ExceptionLogRepository(OneDbContext OneDbContext)
        {
            this.oneDbContext = OneDbContext;
        }

        public void SetDbContextForRelation(OneDbContext OneDbContext)
        {
            this.oneDbContext = OneDbContext;
        }

        public IQueryable<ExceptionLog> GetExceptionLog(Func<ExceptionLog, bool> funcWhere)
        {
            return oneDbContext.ExceptionLog.Where(funcWhere).AsQueryable();
        }

        public string BatchCreateExceptionLog(List<ExceptionLog> ExceptionLogs)
        {
            if (ExceptionLogs.Count == 0) return "ExceptionLog is null";

            try
            {
                oneDbContext.ExceptionLog.AddRange(ExceptionLogs);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateExceptionLog(ExceptionLog ExceptionLog)
        {
            try
            {
                oneDbContext.ExceptionLog.Update(ExceptionLog);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string DeleteExceptionLog(ExceptionLog ExceptionLog)
        {
            try
            {
                oneDbContext.ExceptionLog.Remove(ExceptionLog);
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
