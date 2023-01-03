using System.ComponentModel.DataAnnotations;



namespace WebAPI_JWT.Model
{

	public class Login
	{
		[Key]
		[Required]
		public string? UserName
		{
			get;
			set;
		}
		[Required]
		public string? Password
		{
			get;
			set;
		}
	}
}
