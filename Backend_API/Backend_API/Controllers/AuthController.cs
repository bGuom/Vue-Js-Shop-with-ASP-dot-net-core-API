using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend_API.Constants;
using Backend_API.Models;
using Backend_API.Models.ResourceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AuthController : ControllerBase
    {

        private UserManager<User> UserMgr { get; }
        private SignInManager<User> SignInMgr { get; }
        private JWTSettings JWTSettings { get; }
        public object EntityConstans { get; private set; }

        private readonly DBContext _context;

        public AuthController(UserManager<User> usermanager, SignInManager<User> signinmanager, DBContext logcontext, IOptions<JWTSettings> settings)
        {
            UserMgr = usermanager;
            SignInMgr = signinmanager;
            _context = logcontext;
            JWTSettings = settings.Value;
        }

        // GET: api/Auth/Stat
        [HttpGet("Stat")]
        public ActionResult<int> GetAuthStat()
        {
            var dish_count = _context.Users.Count();
            return dish_count;
        }


        // This is an Example 
        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok(new{
                status = "Auth Server started",
                register = new {
                    type = "POST",
                    route = "/api/auth/register",
                    content_type ="JSON",
                    body =new {
                        UserName = "TestUser",
                        DisplayName = "Test User",
                        Email = "testuser@test.com",
                        Password = "ABCabc123!@#"
                    },
                    response = new
                    {
                        UserName = "TestUser",
                        DisplayName = "Test User",
                        Email = "testuser@test.com",
                        Password = "ABCabc123!@#"
                    },
                },
                adminregister = new
                {
                    type = "POST",
                    authorization = "Bearer",
                    route = "/api/auth/adminregister",
                    content_type = "JSON",
                    body = new
                    {
                        UserName = "TestUser",
                        DisplayName = "Test User",
                        Email = "testuser@test.com",
                        Role = "Admin",
                        Password = "ABCabc123!@#"
                    },
                    response = new
                    {
                        UserName = "TestUser",
                        DisplayName = "Test User",
                        Email = "testuser@test.com",
                        Password = "ABCabc123!@#"
                    },
                },
                getToken = new
                {
                    type = "POST",
                    route = "/api/auth/GetToken",
                    content_type = "JSON",
                    body = new
                    {
                        Email = "testuser@test.com",
                        Password = "ABCabc123!@#"
                    },
                    response = new {
                        thisuser = new UserResourceModel
                        {
                            UserName = "TestUser",
                            DisplayName ="Test User",
                            Email = "testuser@test.com",
                            Role = "User"
                        },
                        token = "BJG273GJDAS73GDAKJDSH.hgJG7hjvj7jhjgu7jhgj.GUTYGu6GV655Yd3gcH",
                        expiration = "30mins"

                    },
                },
                
            });
        }



        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<IActionResult> getinfoAsync()
        {
            string email = User.Claims.First(c => c.Type == "Email").Value;
            User user = await UserMgr.FindByEmailAsync(email);
            var roles = await UserMgr.GetRolesAsync(user);
            var returnuser = new UserResourceModel
            {
                UserName = user.UserName,
                DisplayName =user.DisplayName,
                Role = roles.FirstOrDefault().ToString(),
                Email = user.Email,
            };
            return Ok(returnuser);
        }


        //Generate JWT Tokens for User Login 
        private ActionResult generateToken(User user, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.Secret));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {

                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim("Role",role),

                        };

            var token = new JwtSecurityToken(

                JWTSettings.Issuer,
                JWTSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(JWTSettings.ExpieryTimeInMins),
                signingCredentials: cred

                );

            var results = new
            {
                thisuser = new UserResourceModel { UserId = user.Id, UserName = user.UserName, DisplayName = user.DisplayName, Email = user.Email, Role = role },
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };
            return Ok(results);

        }

                          



        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]AuthBindingModel model)
        {
            if (ModelState.IsValid)
            {

                User user = await UserMgr.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var signInResult = await SignInMgr.CheckPasswordSignInAsync(user, model.Password.ToString(), false);

                    if (signInResult.Succeeded)
                    {
                        var roles = await UserMgr.GetRolesAsync(user);

                        return generateToken(user, roles.FirstOrDefault());

                    }
                    else
                    {
                        var err2 = new { status = "error", message = "Authentication Failed ! Check Email & Password" };
                        return BadRequest(err2);
                    }
                }

                var err = new { status = "error", message = "Could not find a user for given Email!" };
                return BadRequest(err);
            }

            return BadRequest();
        }


       
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
                try
                {

                    User user = await UserMgr.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = model.UserName,
                            DisplayName = model.DisplayName,
                            Email = model.Email,
                           
                        };

                        IdentityResult result = await UserMgr.CreateAsync(user, model.Password);
                        await UserMgr.AddToRoleAsync(user, EntityConstants.Role_SuperAdmin);
                        if (result.Succeeded)
                        {
                        var response = new ResponseModel { Status = "Success", Code = "200", Message = "Account Created", Data = model }; 
                            return Created("", response);
                        }
                        else
                        {
                            
                            var response = new ResponseModel { Status = "Error", Code = "400", Message = "User registration " + result.ToString(), Data = null };
                            return Ok(response);
                        }

                    }
                    else
                    {
                        //User Already exsist
                        
                        var response = new ResponseModel { Status = "Error", Code = "400", Message = "Email/UserName already exsist!" , Data = null };
                        return Ok(response);
                    }
                }
                catch (Exception ex)
                {
                    
                    var response = new ResponseModel { Status = "Error", Code = "400", Message = ex.Message, Data = null };
                    return Ok(response);
                }


        }

       

        [HttpPost("AdminRegister")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AdminRegister(RegisterBindingModel model)
        {
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {

                try
                {

                    User user = await UserMgr.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = model.UserName,
                            DisplayName = model.DisplayName,
                            Email = model.Email,

                        };

                        IdentityResult result = await UserMgr.CreateAsync(user, model.Password);
                        await UserMgr.AddToRoleAsync(user, model.Role);
                        if (result.Succeeded)
                        {
                            return Created("", model);
                        }
                        else
                        {
                            var err = new { status = "error", message = "User registration " + result.ToString() };
                            return BadRequest(err);
                        }

                    }
                    else
                    {
                        //User Already exsist
                        var err = new { status = "error", message = "User already exsist!" };
                        return BadRequest(err);
                    }
                }
                catch (Exception ex)
                {
                    var err = new { status = "error", message = ex.Message };
                    return BadRequest(err);
                }
            }
            else
            {
                return Forbid();
            }

        }



    }
}