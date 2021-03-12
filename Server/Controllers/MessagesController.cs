using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Server.Dtos;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProfanityFilter.Interfaces;
using Ganss.XSS;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BP_OnlineDOD.Server.Controllers
{

    [Route("api/messages")]
    [ApiController]
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
        public ActionResult<IEnumerable<MessageReadDto>> GetAllMessages()
        {
            //Serilog.Log.Information("GET /api/messages");

            var messageItems = _onlineDOD.GetAllMessages();

            return Ok(_mapper.Map<IEnumerable<MessageReadDto>>(messageItems));
        }

        //GET api/messages/{id}
        [HttpGet("{id}", Name = "GetMessageById")]
        public ActionResult<MessageReadDto> GetMessageById(int id)
        {
            //Serilog.Log.Information($"GET /api/messages/{id}");

            var messageItem = _onlineDOD.GetMessageById(id);
            if (messageItem != null)
            {
                return Ok(_mapper.Map<MessageReadDto>(messageItem));
            }

            return NotFound();
        }

        //POST api/messages
        [HttpPost]
        public ActionResult<MessageReadDto> CreateMessage(MessageCreateDto messageCreateDto)
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

            var messageReadDto = _mapper.Map<MessageReadDto>(messageModel);

            //Serilog.Log.Information($"[{this.Request.Host.Host}] POST /api/messages -> ID - {messageReadDto.Id}");

            return CreatedAtRoute(nameof(GetMessageById), new { Id = messageReadDto.Id }, messageReadDto);
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

            //Serilog.Log.Information($"[{this.Request.Host.Host}] PUT /api/messages/{id}");

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

            //Serilog.Log.Information($"[{this.Request.Host.Host}] PATCH /api/messages/{id}");

            return NoContent();
        }

        //DELETE api/messages/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteMessage(int id)
        {

            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _onlineDOD.DeleteMessage(messageModel);
            _onlineDOD.SaveChanges();

            //Serilog.Log.Information($"[{this.Request.Host.Host}] DELETE /api/messages/{id}");

            return NoContent();
        }
    }
}