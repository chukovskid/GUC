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
    [Route("api/users/{userId}/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly UserManager<User> _userManager;

        public NotificationController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> clodinaryConfig, UserManager<User> userManager)
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

        // api/users/{userId}/notifications/{id}
        [HttpGet("{id}", Name = "GetNotification")] // 108
        public async Task<IActionResult> GetNotification(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            string stringId = userId.ToString();
            var user = await _userManager.FindByIdAsync(stringId);

            var notification = await _repo.GetNotification(id);


            return Ok(notification);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var notifications = await _repo.GetNotifications();

            return Ok(notifications);
        }




        [HttpPost]
        public async Task<IActionResult> AddNotification(int userId, NotificationForCreationDto notificationForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); // principov cesto ke bide koristen bidejki ke proveruvam koe ID e logiran
                                       // var userFromRepo = await _repo.GetUser(userId);
                                       // roomForCreation.StudentIds = roomForCreation.StringStudent.Split(',').Select(Int32.Parse).ToList(); 
                                       // roomForCreation.StringStudent = null; //// string Student e trgnat od RoomForCreatinon

            List<User> readByList = new List<User>();
            if (notificationForCreation.ReadBy != null)
            {
                foreach (var student in notificationForCreation.ReadBy)
                {
                    var user = await _userManager.FindByIdAsync(student.ToString());
                    readByList.Add(user);
                } // site koi go imaat procitano postov 

            }

            if (notificationForCreation == null)
            {
                return BadRequest("Sorry, The notification you have entered is empty");
            }

            // Ako nikoj ne go procital pocni da pravis lista
            if (notificationForCreation.ReadBy == null)
            {
                notificationForCreation.ReadBy = new List<int>();
            }
            List<int> Student = notificationForCreation.ReadBy.ToList();
            var notificationsFromRepo = await _repo.GetNotifications();

            notificationForCreation.ReadByCount = 0;
            notificationForCreation.Created = DateTime.Now;

            var notifications = _mapper.Map<Notifications>(notificationForCreation);

            if (notifications.notificationUsers == null)
            {
                notifications.notificationUsers = new List<NotificationUser>();
            }

            // foreach (var user in readByList) // ova bese za da Dodadam Useri koi go procitale
            // {
            //     notificationUser = new NotificationUser
            //     {
            //         NotificationId = notificationForCreation.Id ,
            //         ReaderId = user.Id
            //     };


            //     notifications.notificationUsers.Add(notificationUser);
            // }


            // user.Room = room;

            _repo.Add<Notifications>(notifications);


            if (await _repo.SaveAll())
            {
                var newNotification = await _repo.GetNotification(notifications.Id);
                // var roomToReturn = _mapper.Map<RoomForCreationDto>(room);
                return CreatedAtRoute("GetRoom", new { userId = userId, id = notifications.Id }, newNotification);

            }


            return BadRequest("Ne moze da se dodade Izvestuvanjeto !");

        }





        //Delete//: api/users/{userId}/photos  /id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int userId, int id)
        { // 113

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); // principov cesto ke bide koristen bidejki ke proveruvam koe ID e logiran
            var user = await _repo.GetUser(userId);
            // if (user.Room == null )
            //     return BadRequest("This user doesn't have a room"); // ako nema Room vo userot vrati Unauthorized
            var notificationFromRepo = await _repo.GetNotification(id);

            _repo.Delete(notificationFromRepo);


            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Izvestuvanjeto ne uspea da se izbrise");



        }

        //Delete//: api/users/{userId}/notification/{id}
        // EDIT ROOM..
        [HttpPut("{id}")]
        public async Task<IActionResult> EditNotification(int userId, NotificationForCreationDto notificationForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var theNotification = await _repo.GetNotification(notificationForCreation.Id);

            if (theNotification == null)
            {
                return BadRequest("the room does not exist");
            }

            _mapper.Map(notificationForCreation, theNotification); // Rabotite od userForUpdate STAVI GI VO userFromRepo

            if (await _repo.SaveAll())
                return NoContent();


            throw new Exception($"Updating ROOM used {notificationForCreation.Id} filed to save");

        }
// notification/users
        [HttpGet("users")]
         public async Task<IActionResult> GetClearUsers(int userid)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                 var users = await _repo.GetFullUsers();
            

            return Ok(users);
        }


        [HttpPost("{id}/read")]
        public async Task<IActionResult> AddUserInReadBy(int userid, int id)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userid);
            var notificationFromRepo = await _repo.GetNotification(id);
            notificationFromRepo.ReadByCount++;




            var notificationUser = await _repo.GetNotificationUser(userid, id);
            if (notificationUser != null)
            {
                return Ok();
            }

            notificationUser = new NotificationUser
            {
                NotificationId = id,
                ReaderId = userid
            };

                // notificationFromRepo.notificationUsers.Add(notificationUser);

            // notificationFromRepo.ReadBy.Add(user);
            // notificationFromRepo.DateRead = DateTime.Now; ////////////////// PAMETNO E DA SE DODADE
        _repo.Add<NotificationUser>(notificationUser); // i Dodadi go vo lista Sinhrono


            await _repo.SaveAll();

            return NoContent();
        }

    }
}
