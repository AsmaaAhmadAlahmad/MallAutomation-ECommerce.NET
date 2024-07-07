
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlhamraMallApi.ApiModels;
using AlhamraMall.Domains.Entities;
using AlhamraMall.Data;
using AlhamraMallApi.Repositories;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using AlhamraMallApi.Shared;
namespace AlhamraMallApi.Controllers
{
    // تمت الاستفادة من الرابط التالي
    // https://www.c-sharpcorner.com/article/jwt-token-creation-authentication-and-authorization-in-asp-net-core-6-0-with-po/
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IGenericRepository<User, RegisterModel, LoginModel> genericRepository;

        public AuthenticationController(IConfiguration configuration,
                                        IGenericRepository<User,RegisterModel,LoginModel> genericRepository)
        {
            this.configuration = configuration;
            this.genericRepository = genericRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                // يمكنك هنا تحقق من صحة البيانات المرسلة والتأكد من
                // عدم تكرار البريد الإلكتروني أو اسم المستخدم وغيرها من التحققات اللازمة
                // التأكد من أن اس المستخدم ليس موجود سابقا في الداتا بيز
                var existUser = await genericRepository.GetItemAsync(filterIdAndIsDeleted: c => c.IsDeleted != true && c.Email == registerModel.Email);

                if (existUser != null)
                {

                    return StatusCode(409, new { ErrorMessage = "This email is already registered." });
                }

                else
                {

                    var user = new RegisterModel
                    {
                        UserName = registerModel.UserName.ToLower(),
                        Password = registerModel.Password,
                        Email = registerModel.Email,
                        ConfirmPassword = registerModel.ConfirmPassword,
                    };
                    registerModel.UserName.ToLower();
                    await genericRepository.AddItemAsync(user);
                    await genericRepository.save(); 

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,new { ErrorMessage = "An error occurred while registering the user." });
            }
        }



        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginModel loginMoel)
        {

            var user = await validUserCredenctails(loginMoel);

            if (user != null)
            {
                var token =await GenerateTokenAsync(loginMoel);
                return Ok(token);
            }

            return NotFound(new ApiError
            {
                ErrorCode = "UserNotFound",
                ErrorMessage = "The user account does not exist."
            });
        }




        private async Task<ActionResult> GenerateTokenAsync(LoginModel loginMoel)
        {

            var existUser = await genericRepository.GetItemAsync(filterIdAndIsDeleted: c => c.IsDeleted != true && c.Email == loginMoel.email,includeProperties:"Roles");

            var Cliams = new List<Claim>();
            Cliams.Add(new Claim(ClaimTypes.Name, existUser!.UserName));

            // الحصول على قائمة الادوار المرتبطة بهذا اليوزر لان هذا اليوزر ربما يملك دور او اكثر 
            // وجميعها سيتم اضافتها الى الكليمز 
            var userRoles = existUser.Roles;

            // إضافة الأدوار كـ Claims
            foreach (var role in userRoles)
            {
                Cliams.Add(new Claim(ClaimTypes.Role, role.RoleName));

            }


            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Athentication:secretKey"]));

            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Athentication:Issuer"],
                configuration["Athentication:Audience"],
                Cliams,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(100),
                SigningCredentials);

            var SerializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(SerializedToken);
        }



        // الدالة التالية تتحقق من وجود اليوزر في قاعدة البيانات على ماأظن
        private async Task<User> validUserCredenctails(LoginModel loginModel)
        {

            var currentUser =await genericRepository.GetItemAsync(filterIdAndIsDeleted: c => c.IsDeleted != true && c.Email ==
                loginModel.email && c.Password == loginModel.Password);

            if (currentUser == null)
                return null;

            return currentUser;

        }
    }
}
