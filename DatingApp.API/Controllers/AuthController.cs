using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")] // bidejki ovde imam pisano "api/" bara da imame api/ vo linkot a za [Controller] e smeneto so AUTH!!! bitno
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config; // go koristam za AppSetings da menuvam/ se koristi dole kaj Login Token
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }// constructor. pravam instanca so _repo. a repo ke go dobijam koga ke ja vikaat classava


        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {  //JSON: ovie vrednosti za username i passsword gi dobivam kako JSON file odgovoren e [ApiControler]
           // validate request // staviv ModelDto poradi toa sto nego go cita i avtomatski pretvara od JSON
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))// ako ne postoi pravi go ova!!
                return BadRequest("Username already exists"); // za BadRequest() mi treba : ControllerBase

            User userToCreate = new User
            { // napravi nov User
                Username = userForRegisterDto.Username // go zacuvuvam usernamot vo nov objekt user
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password); // register ti e gotovo polno User so username, passHash i passSalt bez ID

            return StatusCode(201); // CreatedAtRoute( posle ke go sredime) bidejki nemame User od db kako sto sfativ
        }




        // LOGIN 
        [HttpPost("login")]       
        public async Task<ActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // var userFrom Repo ke mi bide gotov model vraten od repo preku login 
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            // sega imam dobien model, User koj moze e praen(null) ako e takov login ke mi return null;
            if (userFromRepo == null)
                return Unauthorized(); // ke vrati deka ne sto e gresno, user ili pass

            var claims = new[] { // pravime Token koj ke go vratime na User
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            }; // go vrakjame ova za da ne ide do DB da vlece info


            //
            // sega ni treba kluc za da go vrati Tokenot

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value)); // ide vo appsettings.json i go bara AppSetings:token vo koj imam Kluc

            //  credentials za SIGN IN (login)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);// key: e security key, algoritmot: go koristime za hash

            var tokenDescriptor= new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // do koga da trae
                SigningCredentials = creds // 3-35.13:00 klucot i algoritamot
            };

            var tokenHandler = new JwtSecurityTokenHandler();// go pravime ova za da mozeme da go pass(pratime) tokenDescripterot! vo nredniov line (var token)

            var token = tokenHandler.CreateToken(tokenDescriptor); // tokenHandler e JwtSecurityTokenHandler()

            
            return Ok(new {
                token = tokenHandler.WriteToken(token) // ovoj token go vrakjame na Klientot// bezz tokenHandler nema da go cita, baska ne postoi writeToken() samo
            });

      
        }


    }
}