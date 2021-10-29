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

        public string BatchCreateTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.AddRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BatchCreateTwoAccountOfThousand(List<TwoAccount> TwoAccounts)
        {
	        try
	        {
		        twoDbContext.BulkInsert(TwoAccounts);
		        return string.Empty;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }
        }

        public string BatchUpdateTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.UpdateRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BatchUpdateTwoAccountOfThousand(List<TwoAccount> TwoAccounts)
        {
	        try
	        {
		        twoDbContext.BulkUpdate(TwoAccounts);
		        return string.Empty;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }
        }

        public string BatchDeleteTwoAccount(List<TwoAccount> TwoAccounts)
        {
            try
            {
                twoDbContext.Account.RemoveRange(TwoAccounts);
                twoDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            twoDbContext.Dispose();
        }
    }
}