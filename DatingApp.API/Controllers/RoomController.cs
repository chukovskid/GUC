using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly UserManager<User> _userManager;

        public RoomController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> clodinaryConfig, UserManager<User> userManager)
        {
            _userManager = userManager;
            _cloudinaryConfig = clodinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account( // zoso pravam nov  ?
            _cloudinaryConfig.Value.CloudName,
            _cloudinaryConfig.Value.ApiKey,
            _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);


        }


        [HttpGet("{id}", Name = "GetRoom")] // 108
        public async Task<IActionResult> GetRoom(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            string stringId = userId.ToString();
            var user = await _userManager.FindByIdAsync(stringId);

            var room = await _repo.GetRoom(id);


            return Ok(room);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var rooms = await _repo.GetRooms();

            return Ok(rooms);
        }




        [HttpPost]
        public async Task<IActionResult> AddRoomForUser(int userId, RoomForCreationDto roomForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); // principov cesto ke bide koristen bidejki ke proveruvam koe ID e logiran
                                       // var userFromRepo = await _repo.GetUser(userId);
                                       // roomForCreation.StudentIds = roomForCreation.StringStudent.Split(',').Select(Int32.Parse).ToList(); 
                                       // roomForCreation.StringStudent = null; //// string Student e trgnat od RoomForCreatinon

            List<User> userList = new List<User>();
            if (roomForCreation.StudentIds != null)
            {
                 foreach (var student in roomForCreation.StudentIds)
            {
                var user = await _userManager.FindByIdAsync(student.ToString());
                userList.Add(user);
            }

            }
            foreach (var student in roomForCreation.StudentIds)
            {
                var user = await _userManager.FindByIdAsync(student.ToString());
                userList.Add(user);
            }

            if (roomForCreation == null)
            {
                return BadRequest("Sorry, The room you have entered is empty");
            }

            // dali Ovoj user ima vekje Room
            if (roomForCreation.StudentIds == null)
            {
                roomForCreation.StudentIds = new List<int>();
            }
            List<int> Student = roomForCreation.StudentIds.ToList();
            var rooms = await _repo.GetRooms();


            foreach (var soba in rooms)
            {
                if (roomForCreation.Number == soba.Number)
                {
                    return BadRequest("Sorry, this Room Number exist by the Id of: " + soba.Id);
                }
            }



            foreach (var soba in rooms)
            {
                if (soba.Student == null)
                {
                    break;
                }
                else
                {
                    foreach (var id in Student)
                    {
                        if (soba.Student.Any(u => u.Id == id))
                            return BadRequest("Sorry, this student has the room: " + soba.Number);
                    }
                }

            }

            roomForCreation.occupiedBeds = roomForCreation.StudentIds.Count();

            var room = _mapper.Map<Room>(roomForCreation);

            if (room.Student == null)
            {
                room.Student = new List<User>();
            }

            foreach (var user in userList)
            {
                room.Student.Add(user);
            }


            // user.Room = room;

            _repo.Add<Room>(room);


            if (await _repo.SaveAll())
            {
                var newRoom = await _repo.GetRoom(room.Id);
                // var roomToReturn = _mapper.Map<RoomForCreationDto>(room);
                return CreatedAtRoute("GetRoom", new { userId = userId, id = room.Id }, newRoom);

            }


            return BadRequest("Ne moze da se dodade Sobata!");

        }





        //Delete//: api/users/{userId}/photos  /id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int userId, int id)
        { // 113

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); // principov cesto ke bide koristen bidejki ke proveruvam koe ID e logiran
            var user = await _repo.GetUser(userId);
            // if (user.Room == null )
            //     return BadRequest("This user doesn't have a room"); // ako nema Room vo userot vrati Unauthorized
            var roomFromRepo = await _repo.GetRoom(id);

            _repo.Delete(roomFromRepo);


            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Slikata ne uspea da se izbrise");



        }

        //Delete//: api/users/{userId}/photos/{id}
        // EDIT ROOM..
        [HttpPut("{id}")]
        public async Task<IActionResult> EditRoom(int userId, RoomForCreationDto roomForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var theRoom = await _repo.GetRoom(roomForCreation.Id);

            if (theRoom == null)
            {
                return BadRequest("the room does not exist");
            }

            _mapper.Map(roomForCreation, theRoom); // Rabotite od userForUpdate STAVI GI VO userFromRepo

            if (await _repo.SaveAll())
                return NoContent();


            throw new Exception($"Updating ROOM used {roomForCreation.Id} filed to save");

        }


    }
}
