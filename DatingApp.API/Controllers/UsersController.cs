using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LastUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet] // api/users
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            // Informaciite od Header za Id i Gender gi dodavam vo UserParams za vo DatingRepo da mozam da isfilrtiram

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // go zimam ID od User.Claims
            userParams.UserId = currentUserId;

            var userFromRepo = await _repo.GetUser(currentUserId); // go zimam userov da vidam dali ima Gender staveno

            if (string.IsNullOrEmpty(userParams.Gender))
            { // ako NEMA preferenca sakame da go stavime LOGICNO SPROTIVNO OD TOA STO E
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams); // GetUsers vrakja -> PagedList<T> vo koj ima Userite kako IEnumerable + userParams
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // vo Headerot sakam da gi imam UserParams
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }



        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user); // _mapper go koritsam za da fo isfiltrira pretpostavuvam
            return Ok(userToReturn);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); // principov cesto ke bide koristen bidejki ke proveruvam koe ID e logiran

            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdate, userFromRepo); // Rabotite od userForUpdate STAVI GI VO userFromRepo

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating used {id} filed to save");


        }
    }
}