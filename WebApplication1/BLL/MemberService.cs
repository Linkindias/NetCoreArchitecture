using System;
using System.Linq;
using Base;
using BLL.Model;
using DAL.Repo;

namespace BLL
{
    public class MemberService: IDisposable
    {
        public Guid guid { get; set; }

        private OneAccountRepository _oneAccountRepo;
        private TwoAccountRepository _twoAccountRepo;

        public MemberService(OneAccountRepository oneAccountRepo, TwoAccountRepository twoAccountRepository)
        {
            this._oneAccountRepo = oneAccountRepo;
            this._twoAccountRepo = twoAccountRepository;
            this.guid = Guid.NewGuid();
        }

        public AccountVM GetAccount(string account)
        {
            return (from oneAcocunt in _oneAccountRepo.GetOneAccount(o => o.Account1.ToLower() == account.ToLower())
                    join twoAccount in _twoAccountRepo.GetTwoAccount(o => o == o) on oneAcocunt.Account1 equals twoAccount.Account1
                    select new AccountVM()
                    {
                        oneAccount = oneAcocunt.Account1,
                        oneName =  oneAcocunt.Name,
                        twoAccount = twoAccount.Account1,
                        twoName =  twoAccount.Name
                    }).FirstOrDefault();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        public (string msg, Member member) LogIn(string account, string password)
        {
            return (string.Empty, new Member(0,"test",0,"user",0,"testdepart", "menus"));
        }

        public void Dispose()
        {
            _oneAccountRepo.Dispose();
            _twoAccountRepo.Dispose();
        }
    }
}