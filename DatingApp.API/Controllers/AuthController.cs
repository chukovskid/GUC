using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.API.Controllers
{
    
    [Route("api/[controller]")] 
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config; // go koristam za AppSetings da menuvam/ se koristi  Login Token
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper; // 117
            _config = config;
            _repo = repo;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {  
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists"); 

            var userToCreate = _mapper.Map<User>(userForRegisterDto);
           
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password); 
           
            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser); 

            return CreatedAtRoute("GetUser", new {Controller = "Users", id = createdUser.Id}, userToReturn); 
        }




        // LOGIN 
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized(); // ke vrati deka ne sto e gresno, user ili pass
            var claims = new[] { // pravime Token koj ke go vratime na User
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            }; // go vrakjame ova za da ne ide do DB da vlece info


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

            var user = _mapper.Map<UserForListDto>(userFromRepo); // go koristam UserForListDto bidejki e najmalo dto od user i ima PhotoURL// 117


            return Ok(new
            {
                token = tokenHandler.WriteToken(token), // ovoj token go vrakjame na Klientot// bezz tokenHandler nema da go cita, baska ne postoi writeToken() samo
                user // 117
            });


        }


    }
}