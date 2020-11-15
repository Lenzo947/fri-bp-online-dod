using AutoMapper;
using BP_OnlineDOD_Server.Data;
using BP_OnlineDOD_Server.Dtos;
using BP_OnlineDOD_Server.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD_Server.Controllers
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
            var messageItems = _onlineDOD.GetAllMessages();

            return Ok(_mapper.Map<IEnumerable<MessageReadDto>>(messageItems));
        }

        //GET api/messages/{id}
        [HttpGet("{id}", Name = "GetMessageById")]
        public ActionResult<MessageReadDto> GetMessageById(int id)
        {
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

            return CreatedAtRoute(nameof(GetMessageById), new { Id = messageReadDto.Id }, messageReadDto);
        }

        //PUT api/messages/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateMessage(int id, MessageUpdateDto messageUpdateDto)
        {
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

            return NoContent();
        }
    }
}