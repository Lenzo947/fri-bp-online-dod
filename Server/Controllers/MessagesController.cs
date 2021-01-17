using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Server.Dtos;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD.Server.Controllers
{

    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;
        private readonly IMapper _mapper;

        public MessagesController(IOnlineDOD onlineDOD, IMapper mapper)
        {
            _onlineDOD = onlineDOD;
            _mapper = mapper;
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
            var messageModel = _mapper.Map<Message>(messageCreateDto);
            _onlineDOD.CreateMessage(messageModel);
            _onlineDOD.SaveChanges();

            var messageReadDto = _mapper.Map<MessageReadDto>(messageModel);

            Serilog.Log.Information($"POST /api/messages -> ID - {messageReadDto.Id}");

            return CreatedAtRoute(nameof(GetMessageById), new { Id = messageReadDto.Id }, messageReadDto);
        }

        //PUT api/messages/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateMessage(int id, MessageUpdateDto messageUpdateDto)
        {
            Serilog.Log.Information($"PUT /api/messages/{id}");

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
            Serilog.Log.Information($"PATCH /api/messages/{id}");

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

        //DELETE api/messages/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteMessage(int id)
        {
            Serilog.Log.Information($"DELETE /api/messages/{id}");

            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _onlineDOD.DeleteMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }
    }
}