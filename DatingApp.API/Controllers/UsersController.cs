using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LastUserActivity))]
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

         [HttpGet("full")] // api/users
        public async Task<IActionResult> GetFullUsers([FromQuery] UserParams userParams)
        {

            var users = await _repo.GetFullUsers();

            return Ok(users);
        }

        [HttpGet] // api/users
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // go zimam ID od User.Claims
            userParams.UserId = currentUserId;

            var userFromRepo = await _repo.GetUser(currentUserId); 

            if (string.IsNullOrEmpty(userParams.Gender))
            { // ako NEMA preferenca sakame da go stavime LOGICNO SPROTIVNO OD TOA
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

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); 

            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdate, userFromRepo); // Rabotite od userForUpdate STAVI GI VO userFromRepo

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating used {id} filed to save");


        }

        [HttpPost("{id}/like/{recipientId}")] // Ke Followneme nekoj
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized(); 

            var like = await _repo.GetLike(id, recipientId);

            if(like != null){
                return BadRequest("You have already liked this user");
            }

            if(await _repo.GetUser(recipientId) == null){
             return NotFound();
            } 
                // ako se e dobro napravi mi nov Like
                like = new Like
                {
                    LikerId = id,
                    LikeeId = recipientId
                };

            _repo.Add<Like>(like); // i Dodadi go vo lista Sinhrono

            if(await _repo.SaveAll()){
             return Ok();
            }

            return BadRequest("Failed to Like the user");
        }
    }
}