using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_API.Constants;
using Backend_API.Models;
using Backend_API.Models.ResourceModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {

        private RoleManager<UserRole> RoleMgr { get; }


        public RoleController(UserManager<User> usermanager, RoleManager<UserRole> roleManager, SignInManager<User> signinmanager, DBContext logcontext)
        {

            RoleMgr = roleManager;

            //Generate UserRoles in DB
            var userRoles = new List<string> {
                EntityConstants.Role_SuperAdmin,
                EntityConstants.Role_Admin,
                EntityConstants.Role_RegisteredUser,
                EntityConstants.Role_GuestUser,
                };

            var dbRoles = RoleMgr.Roles.Select(s => s.Name).ToList();
            var notInDB = userRoles.Where(w => !dbRoles.Contains(w)).ToList();

            notInDB.ForEach(f => {
                var res = RoleMgr.CreateAsync(new UserRole { Name =f, Description =f, DateCreated = DateTime.Now}).Result;
            });
        }



        // GET: api/Role
        [HttpGet]
        public ActionResult<List<RoleResourceModel>> GetRoles()
        {
            var data = RoleMgr.Roles.ToList();

            List<RoleResourceModel> All_Roles = new List<RoleResourceModel>();

            foreach(var role in data)
            {
                RoleResourceModel user_role = new RoleResourceModel
                {
                    RoleId =role.Id,
                    Name =role.Name,
                    Description = role.Description,
                    DateCreated =role.DateCreated
                };
                All_Roles.Add(user_role);

            };
            return Ok(All_Roles);
        }

        // GET: Roles/5
        [HttpGet("{id}")]
        public ActionResult<RoleResourceModel> GetRole(Guid? id)
        {
            if (id == null)
                return BadRequest(new { Error = "ID can not be null"});

            var role = RoleMgr.Roles.Where(f => f.Id == id).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            RoleResourceModel user_role = new RoleResourceModel
            {
                RoleId = role.Id,
                Name = role.Name,
                Description = role.Description,
                DateCreated = role.DateCreated
            };

            return Ok(user_role);
        }

    }
}