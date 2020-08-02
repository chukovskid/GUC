using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        public AdminController(DataContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")] // 5000/api/admin/usersWithRoles
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _context.Users
            .OrderBy(x => x.UserName)
            .Select(user => new // so Select mozam da gi menuvam listite
            {
                Id = user.Id,
                userName = user.UserName,
                Roles = (from userRole in user.UserRoles // za sekoj Role vo User
                         join role in _context.Roles //  spoi gi so Role sto postoi vo Roles tabelata
                         on userRole.RoleId //  no samo tie kade UserId
                         equals role.Id // e isto so Role Id
                         select role.Name) // tie sto ke ostanat vrati mi gi nivnite iminja
                            .ToList()
            }).ToListAsync();

            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{UserName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = roleEditDto.RoleNames;

            // selectedRoles = selectedRoles != null ? selectedRoles : new String[] {};
            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles)); // stavi gi site RoleNames osven tie sto vekje gi ima, da ne se duplira
            if (!result.Succeeded)
            {
                return BadRequest("Failed on adding the roles");
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles)); // trgni gi site RoleNames osven Novite
            if (!result.Succeeded)
            {
                return BadRequest("failed on removing the old Roles");
            }


            var newRoles = await _userManager.GetRolesAsync(user);
            return Ok(newRoles);
    
        }




        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")] // 5000/api/admin/photosForModeration
        public IActionResult GetPhotosForModerator()
        {
            return Ok(" Admins and Moderators can see this");
        }


    }
}