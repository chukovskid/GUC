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
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessageController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }


        // Get Message
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }


        // Create Message
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreaton)
        {

            var sender = await _repo.GetUser(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();


            messageForCreaton.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreaton.RecipientId);
            if (recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreaton);


            if (message == null)
                return NotFound();

            _repo.Add(message);

            var messageToReturn = _mapper.Map<MessageForReturnDto>(message);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageToReturn);


            throw new Exception("Creating the message failed on save");
        }


        // Get Messages
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageForReturnDto>>(messagesFromRepo);

            //  Vo Header, za da gi imam informaciive za posle
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
            messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }


        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();


            var messageThreadFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageForReturnDto>>(messageThreadFromRepo); 


            return Ok(messagesToReturn);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDelete = true;

            if (messageFromRepo.RecipientDelete == true && messageFromRepo.SenderDeleted == true)
            {
                _repo.Delete(messageFromRepo);
            }

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Error deleting the message");

        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userid, int id)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.RecipientId != userid)
                return Unauthorized();

            messageFromRepo.IsRead = true;
            messageFromRepo.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();
        }

    }
}