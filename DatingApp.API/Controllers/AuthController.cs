using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        // private readonly IAuthRepository _repo;
        private readonly IConfiguration _config; // go koristam za AppSetings da menuvam/ se koristi  Login Token
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController( IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager; // 203
            _userManager = userManager; // 203
            _mapper = mapper; // 117
            _config = config;
            // _repo = repo;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password); // 204

            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

            if (result.Succeeded)
            {
             return CreatedAtRoute("GetUser", new { Controller = "Users", id = userToCreate.Id }, userToReturn);

            }

            return BadRequest(result.Errors);

        }




        // LOGIN 
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName); // koga ke vnese username da go najde userot
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false); // da proba SignIn

            if (result.Succeeded)
            {
                var AppUser = _mapper.Map<UserForListDto>(user); // go koristam UserForListDto bidejki e najmalo dto od user i ima PhotoURL// 117

                return Ok(new
                {
                    token = this.GenerateJwtToken(user).Result, // ovoj token go vrakjame na Klientot// bezz tokenHandler nema da go cita, baska ne postoi writeToken() samo
                    user = AppUser // 117
                });

            }

            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        { // 203
            var claims = new List<Claim> { // pravime Token koj ke go vratime na User
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            }; // go vrakjame ova za da ne ide do DB da vlece info

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                 claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // sega ni treba kluc za da go vrati Tokenot
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value)); // ide vo appsettings.json i go bara AppSetings:token vo koj imam Kluc

            //  credentials za SIGN IN (login)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);// key: e security key, algoritmot: go koristime za hash

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // do koga da trae
                SigningCredentials = creds // 3-35.13:00 klucot i algoritamot
            };

            var tokenHandler = new JwtSecurityTokenHandler();// go pravime ova za da mozeme da go pass(pratime) tokenDescripterot! vo nredniov line (var token)

            var token = tokenHandler.CreateToken(tokenDescriptor); // tokenHandler e JwtSecurityTokenHandler()

            return tokenHandler.WriteToken(token);
        }


    }
}