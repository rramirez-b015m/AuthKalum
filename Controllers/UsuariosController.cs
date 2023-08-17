using KalumAuthManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KalumAuthManagement.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace KalumAuthManagement.Controllers
{
    [ApiController]
     [Route("kalum-auth/v1/usuarios")]
     [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ApplicationDbContext ApplicationDbContext;
        public UsuariosController(UserManager<ApplicationUser>_UserManager, ApplicationDbContext _ApplicationDbContext )
        {
            this.UserManager = _UserManager;
            this.ApplicationDbContext = _ApplicationDbContext;

        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>>Get()
        {
            List<ApplicationUser> users = await UserManager.Users.ToListAsync();
            return users;
        }

         [HttpGet("search/{id}",Name ="GetUsuario")]
        public async Task<ActionResult<ApplicationUser>> Get(string id)
        {
            var usuario = await UserManager.FindByIdAsync(id);
            if(usuario != null)
            {
                return usuario;
            }
            return NotFound();

        }

        
    }
}