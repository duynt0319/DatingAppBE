using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWord _uow;

        public MessagesController( IMapper mapper, IUnitOfWord uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.GetUsername();

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                return BadRequest("You cant send messages to yourself");
            }
            var sender = await _uow.UserRepository.GetUserByUserNameAsync(userName);
            var recipient = await _uow.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };
            _uow.MessageRepository.AddMessage(message);
            if (await _uow.Complete())
            {
                return Ok(_mapper.Map<MessageDto>(message));
            }
            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();
            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

            return messages;
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _uow.MessageRepository.GetMessage(id);

            if (message.SenderUserName != username && message.SenderUserName != username)
                return Unauthorized();

            if (message.SenderUserName == username) message.SenderDeleted = true;
            if (message.RecipientUserName == username) message.RecipientDeleted = true;
            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _uow.MessageRepository.RemoveMessage(message);
            }

            if(await _uow.Complete()) return Ok();
            return BadRequest("Problem deleting the message");
        }
    }
}
