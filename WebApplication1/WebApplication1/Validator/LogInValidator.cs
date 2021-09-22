using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validator
{
	public class LogInValidator:AbstractValidator<LogInInputModel>
	{
		public LogInValidator()
		{
			RuleFor(x => x.account).NotEmpty();
			RuleFor(x => x.password).NotEmpty();
			RuleFor(x => x.account).Length(0,20);
		}
	}
}