using System;
using System.Collections.Generic;
using System.Text;
using DAL.Table;
using AutoMapper;
using DAL;

namespace BLL.MapperModel
{
	public class MappingAccount : Profile
	{
		public MappingAccount()
		{
			CreateMap<OneAccount, PersonInfoVM>()
				.ForMember(dest => dest.oneAccount,o => o.MapFrom(s => s.Account1))
				.ForMember(dest => dest.twoAccount, opt => opt.Ignore())
				.ForMember(dest => dest.tel, opt => opt.Ignore())
				.ForMember(dest => dest.address, opt => opt.Ignore())
				.ForMember(dest => dest.remark, opt => opt.Ignore());
			CreateMap<TwoAccount, PersonInfoVM>()
				.ForMember(dest => dest.sex, opt => opt.Ignore())
				.ForMember(dest => dest.oneAccount, opt => opt.Ignore())
				.ForMember(dest => dest.name, opt => opt.Ignore())
				.ForMember(dest => dest.jobTitle, opt => opt.Ignore())
				.ForMember(dest => dest.phone, opt => opt.Ignore())
				.ForMember(dest => dest.email, opt => opt.Ignore());
		}	
	}
}
