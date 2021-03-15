using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Server.Helpers;
using BP_OnlineDOD.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly OnlineDODContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UsersController(OnlineDODContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);
            return await queryable.Paginate(paginationDTO)
                .Select(x => new UserDTO { Email = x.Email, UserId = x.Id }).ToListAsync();
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDTO>>> Get()
        {
            return await context.Roles
                .Select(x => new RoleDTO { RoleName = x.Name }).ToListAsync();
        }

        [HttpPost("assign-role")]
        public async Task<ActionResult> AssignRole(EditRoleDTO editRoleDTO)
        {
            if (editRoleDTO.RoleName == "Admin")
            {
                return Unauthorized();
            }

            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);
            await userManager.AddToRoleAsync(user, editRoleDTO.RoleName);
            //await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            return NoContent();
        }

        [HttpPost("remove-role")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRoleDTO)
        {
            if (editRoleDTO.RoleName == "Admin")
            {
                return Unauthorized();
            }

            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);
            await userManager.RemoveFromRoleAsync(user, editRoleDTO.RoleName);
            //await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            return NoContent();
        }
    }
}
