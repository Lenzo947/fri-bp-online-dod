using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProfanityFilter.Interfaces;
using Ganss.XSS;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BP_OnlineDOD.Server.Controllers
{

    [ApiController]
    [Route("api/messages")]
    //[Authorize(Roles = "Admin")]
    public class MessagesController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;
        private readonly IMapper _mapper;
        private readonly IProfanityFilter _profanityFilter;
        private readonly IHtmlSanitizer _htmlSanitizer;

        public MessagesController(IOnlineDOD onlineDOD, IMapper mapper, IHtmlSanitizer htmlSanitizer, IProfanityFilter profanityFilter)
        {
            _onlineDOD = onlineDOD;
            _mapper = mapper;
            _profanityFilter = profanityFilter;
            _htmlSanitizer = htmlSanitizer;
        }

        //GET api/messages
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Message>> GetAllMessages()
        {
            ICollection<Message> messageItems;

            if(!User.IsInRole("Admin")) // <--- INVERT CONDITION AFTER ROLES ARE IMPLEMENTED
            {
                messageItems = _onlineDOD.GetAllMessagesWithDeleted();
            }
            else
            {
                messageItems = _onlineDOD.GetAllMessages();
            }

            return Ok(messageItems);
        }

        //GET api/messages/{id}
        [HttpGet("{id}", Name = "GetMessageById")]
        [AllowAnonymous]
        public ActionResult<Message> GetMessageById(int id)
        {
            var messageItem = _onlineDOD.GetMessageById(id);
            if (messageItem != null)
            {
                if (messageItem.Deleted)
                {
                    if (!User.IsInRole("Admin"))
                    {
                        return NotFound();
                    }
                }

                return Ok(messageItem);
            }

            return NotFound();
        }

        //POST api/messages
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<Message> CreateMessage(MessageCreateDto messageCreateDto)
        {
            messageCreateDto.Text =  _htmlSanitizer.Sanitize(_profanityFilter.CensorString(messageCreateDto.Text));
            messageCreateDto.Author = _htmlSanitizer.Sanitize(_profanityFilter.CensorString(messageCreateDto.Author));

            if (messageCreateDto.Text == "")
            {
                messageCreateDto.Text = "<prázdna po vyfiltrovaní>";
                messageCreateDto.Deleted = true;
            }

            var messageModel = _mapper.Map<Message>(messageCreateDto);
            _onlineDOD.CreateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return CreatedAtRoute(nameof(GetMessageById), new { Id = messageModel.Id }, messageModel);
        }

        //PUT api/messages/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateMessage(int id, MessageUpdateDto messageUpdateDto)
        {
            messageUpdateDto.Text = _profanityFilter.CensorString(messageUpdateDto.Text);

            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _mapper.Map(messageUpdateDto, messageModel);

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        //PATCH api/messages/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialMessageUpdate(int id, JsonPatchDocument<MessageUpdateDto> patchDoc)
        {
            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            var messageToPatch = _mapper.Map<MessageUpdateDto>(messageModel);
            patchDoc.ApplyTo(messageToPatch, ModelState);

            if (!TryValidateModel(messageToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(messageToPatch, messageModel);

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        [HttpDelete("hide/{id}")]
        [AllowAnonymous] // <-- DELETE THIS AFTER ROLES ARE IMPLEMENTED
        public ActionResult HideMessage(int id)
        {

            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _onlineDOD.HideMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        [HttpDelete("renew/{id}")]
        [AllowAnonymous] // <-- DELETE THIS AFTER ROLES ARE IMPLEMENTED
        public ActionResult RenewMessage(int id)
        {

            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _onlineDOD.RenewMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        [HttpPost("upvote/{id}")]
        [AllowAnonymous]
        public ActionResult<Message> UpvoteMessage(int id)
        {
            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            messageModel.ThumbsUpCount++;

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        [HttpPost("cancel-upvote/{id}")]
        [AllowAnonymous]
        public ActionResult<Message> CancelUpvote(int id)
        {
            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            messageModel.ThumbsUpCount--;

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }
    }
}