using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base;
using BLL.MapperModel;
using BLL.Model;
using DAL.Repo;
using DAL.Table;
using LinqKit;

namespace BLL
{
    public class MemberTestService: IDisposable
    {
        public Guid guid { get; set; }

        private IMapper iMapper;
        private OneAccountRepository _oneAccountRepo;
        private TwoAccountRepository _twoAccountRepo;

        public MemberTestService(OneAccountRepository oneAccountRepo, TwoAccountRepository twoAccountRepository, IMapper iMapper)
        {
            this._oneAccountRepo = oneAccountRepo;
            this._twoAccountRepo = twoAccountRepository;
            this.iMapper = iMapper;
            this.guid = Guid.NewGuid();
        }

        public OneAccount TestLinqKitPredicate(Member member)
        {
	        var pred = PredicateBuilder.New<OneAccount>(true);

	        if (member != null)
	        {
		        if (!string.IsNullOrWhiteSpace(member.Account))
		        {
			        pred = pred.And(p => p.Account1.ToLower() == member.Account.ToLower());
		        }
		        if (!string.IsNullOrWhiteSpace(member.Name))
		        {
			        pred = pred.And(p => p.Name.Contains(member.Name));
		        }
		        if (!string.IsNullOrWhiteSpace(member.RoleName))
		        {
			        pred = pred.And(p => p.JobTitle.Contains(member.RoleName));
		        }
		        if (!string.IsNullOrWhiteSpace(member.DepartmentName))
		        {
			        pred = pred.And(p => p.Email.Contains(member.DepartmentName));
		        }
	        }
			return _oneAccountRepo.GetOneAccount(pred.Compile()).FirstOrDefault();
        }

        public PersonInfoVM GetAccount(string account)
        {
			//return _oneAccountRepo.GetOneAccount(o => o.Account1.ToLower() == account.ToLower())
			//                        .Join(_twoAccountRepo.GetTwoAccount(p => p == p), o => o.Account1, p => p.Account1, (o, p) =>
			//                             new PersonInfoVM()
			//                             {
			//                                 oneAccount = o.Account1,
			//                                 oneName = o.Name,
			//                                 sex = o.Sex == (int)Enums.Sex.Male ? Enums.Sex.Male.ToString() : Enums.Sex.Female.ToString(),
			//                                 twoAccount = p.Account1,
			//                                 twoName = o.Name
			//                             })
			//                        .FirstOrDefault();

			//return (from oneAccount in _oneAccountRepo.GetOneAccount(o => o.Account1.ToLower() == account.ToLower())
			//          join twoAccount in _twoAccountRepo.GetTwoAccount(o => o == o) on oneAccount.Account1 equals twoAccount.Account1
			//          select new PersonInfoVM()
			//          {
			//           oneAccount = oneAccount.Account1,
			//           oneName = oneAccount.Name,
			//           sex = oneAccount.Sex == (int)Enums.Sex.Male ? Enums.Sex.Male.ToString() : Enums.Sex.Female.ToString(),
			//           twoAccount = twoAccount.Account1,
			//           twoName = twoAccount.Name
			//          }).FirstOrDefault();

			//throw new Exception("test"); test exceptionAttribute

			var test = (from oneAccount in _oneAccountRepo.GetOneAccount(o => o.Account1.ToLower() == account.ToLower())
						 join twoAccount in _twoAccountRepo.GetTwoAccount(o => o == o) on oneAccount.Account1 equals twoAccount
							 .Account1
						 select oneAccount).FirstOrDefault();
			//.ProjectTo<PersonInfoVM>(iMapper.ConfigurationProvider)
			//.FirstOrDefault();
			return iMapper.Map<PersonInfoVM>(test);
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