using API.Data;
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class MessageController : BaseApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        public MessageController(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;

        }

        [HttpPost]

        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();
            if (username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You Can not Sent message to Yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return BadRequest("User Not Found");
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok("message Sent");

            return BadRequest("message not sent");
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessaegeForUser([FromQuery] MessageParams messageParams)

        {
            messageParams.Username = User.GetUserName();
            var messages = await _messageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage,messages.PageSize, messages.TotalCount,messages.TotalPages);
            return messages;
        }

        [HttpGet("thred/{username}")]

        public async Task <ActionResult<IEnumerable<MessageDto>>> GetMessageThreds(string username)

        {
            var currentUserName = User.GetUserName();

            return Ok(await _messageRepository.GetMessageThred(currentUserName , username));
        }

        [HttpDelete("{id}")]

        public async Task <ActionResult> DeleteMessagecall(int id)
        {
            var username = User.GetUserName();
            var message =await _messageRepository.GetMessage(id);
            if(message.Sender.UserName != username && message.Recipient.UserName != username) return Unauthorized();

            if(message.Sender.UserName == username) message.SenderDelete = true;
            if(message.Recipient.UserName == username) message.RecipientDelete = true;

            if(message.SenderDelete && message.RecipientDelete) _messageRepository.DeleteMessage(message);

            if(await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("probem Deleting msg");

        }
    }
}