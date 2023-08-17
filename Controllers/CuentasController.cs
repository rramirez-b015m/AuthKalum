using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KalumAuthManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KalumAuthManagement.DTOs;

namespace KalumAuthManagement.Controllers
{
    [Route("kalum-auth/v1/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> Usermanager;

        private readonly IConfiguration Configuration;



        public CuentasController(IConfiguration _Configuration, SignInManager<ApplicationUser> _SignInManager, UserManager<ApplicationUser> _UserManager)
        {
            this.SignInManager = _SignInManager;
            this.Usermanager = _UserManager;
            this.Configuration = _Configuration;

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginDTO value)
        {
            var login = await SignInManager.PasswordSignInAsync(value.Username, value.Password, isPersistent: false, lockoutOnFailure: false);
            if (login.Succeeded)
            {
                var usuario = await Usermanager.FindByNameAsync(value.Username);
                var roles = await Usermanager.GetRolesAsync(usuario);
                return BuildToken(usuario, roles);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "El login es invalido");
                return BadRequest(ModelState);

            }


        }

        [HttpPost("crear")]
        public async Task<ActionResult<UserToken>> Create([FromBody] UserInfo value)
        {
            var userinfo = new ApplicationUser { UserName = value.Username, Email = value.Email };
            var newUser = await Usermanager.CreateAsync(userinfo, value.Password);

            if (newUser.Succeeded)
            {
                await Usermanager.AddToRoleAsync(userinfo, value.Roles.ElementAt(0));
                return BuildToken(userinfo, value.Roles != null ? value.Roles : new List<string>());
            }
            else
            {
                return BadRequest("La informacion enviada no es correcta");
            }

        }

        [HttpPost("finish-register")]
        public async Task<ActionResult<UserToken>> FinishRegister([FromBody] FinishRegisterDTO request)
        {
            var user = await this.Usermanager.FindByEmailAsync(request.Email); if (user != null)
            {
                user.identificationId = request.IdentificationId; await this.Usermanager.UpdateAsync(user);
                await Usermanager.RemoveFromRolesAsync(user, new string[] { "ROLE_USER" });
                await Usermanager.AddToRoleAsync(user, "ROLE_CANDIDATE");
                return StatusCode(201, BuildToken(user, new string[] { "ROLE_CANDIDATE" }));
            }
            else
            {
                var response = new AccountResponseDTO()
                { StatusCode = 404, Message = $"No existe el correo {request.Email}" };
                return StatusCode(404, response);

            }
        }
        private UserToken BuildToken(ApplicationUser user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("api","kalumAuth"),
                new Claim("username", user.UserName),
                new Claim("email",user.Email),
                new Claim("identificationId",user.identificationId != null ? user.identificationId: "0"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));

            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken
            (

                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }





    }
}