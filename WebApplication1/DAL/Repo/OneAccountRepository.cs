﻿using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using DAL.Table;

namespace DAL.Repo
{
    public class OneAccountRepository : IDisposable
    {
        private OneDbContext oneDbContext;

        public OneAccountRepository(OneDbContext oneDbContext)
        {
            this.oneDbContext = oneDbContext;
        }

        public void SetDbContextForRelation(OneDbContext oneDbContext)
        {
            this.oneDbContext = oneDbContext;
        }

        public IQueryable<OneAccount> GetOneAccount(Func<OneAccount, bool> funcWhere)
        {
            return oneDbContext.Account.Where(funcWhere).AsQueryable();
        }

        public string BatchCreateOneAccount(List<OneAccount> OneAccounts)
        {
            try
            {
                oneDbContext.Account.AddRange(OneAccounts);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BatchCreateOneAccountOfThousand(List<OneAccount> OneAccounts)
        {
	        try
	        {
		        oneDbContext.BulkInsert(OneAccounts);
		        return string.Empty;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }
        }

        public string BatchUpdateOneAccount(List<OneAccount> OneAccounts)
        {
            try
            {
                oneDbContext.Account.UpdateRange(OneAccounts);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BatchUpdateOneAccountOfThousand(List<OneAccount> OneAccounts)
        {
	        try
	        {
		        oneDbContext.BulkUpdate(OneAccounts);
		        return string.Empty;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }
        }

        public string BatchDeleteOneAccount(List<OneAccount> OneAccounts)
        {
            try
            {
                oneDbContext.Account.RemoveRange(OneAccounts);
                oneDbContext.SaveChanges();
                return string.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BatchDeleteOneAccountOfThousand(List<OneAccount> OneAccounts)
        {
	        try
	        {
		        oneDbContext.BulkDelete(OneAccounts);
		        return string.Empty;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }
        }

        public void Dispose()
        {
            oneDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}