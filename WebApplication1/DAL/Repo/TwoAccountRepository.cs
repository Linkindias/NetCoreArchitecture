using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using DAL.Table;

namespace DAL.Repo
{
    public class TwoAccountRepository : IDisposable
    {
        private TwoDbContext twoDbContext;

        public TwoAccountRepository(TwoDbContext TwoDbContext)
        {
            this.twoDbContext = TwoDbContext;
        }

        public void SetDbContextForRelation(TwoDbContext TwoDbContext)
        {
            this.twoDbContext = TwoDbContext;
        }

        public IQueryable<TwoAccount> GetTwoAccount(Func<TwoAccount, bool> funcWhere)
        {
            return twoDbContext.Account.Where(funcWhere).AsQueryable();
        }

        private string BatchCreateTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.AddRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string BatchUpdateTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.UpdateRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string BatchDeleteTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.RemoveRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void Dispose()
        {
            twoDbContext.Dispose();
        }
    }
}