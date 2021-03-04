using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Server.Dtos;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD.Server.Controllers
{
    [Route("api/attachments")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;
        private readonly IMapper _mapper;

        public AttachmentsController(IOnlineDOD onlineDOD, IMapper mapper)
        {
            _onlineDOD = onlineDOD;
            _mapper = mapper;
        }

        //GET api/attachments
        [HttpGet]
        public ActionResult<IEnumerable<AttachmentReadDto>> GetAllAttachments()
        {
            var attachmentItems = _onlineDOD.GetAllAttachments();

            return Ok(_mapper.Map<IEnumerable<AttachmentReadDto>>(attachmentItems));
        }

        //GET api/attachments/{id}
        [HttpGet("{id}", Name = "GetAttachmentById")]
        public ActionResult<AttachmentReadDto> GetAttachmentById(int id)
        {
            var attachmentItem = _onlineDOD.GetAttachmentById(id);
            if (attachmentItem != null)
            {
                return Ok(_mapper.Map<AttachmentReadDto>(attachmentItem));
            }

            return NotFound();
        }

        //POST api/attachments
        [HttpPost]
        public ActionResult<AttachmentReadDto> CreateAttachment(AttachmentCreateDto attachmentCreateDto)
        {
            var attachmentModel = _mapper.Map<Attachment>(attachmentCreateDto);
            _onlineDOD.CreateAttachment(attachmentModel);
            _onlineDOD.SaveChanges();

            var attachmentReadDto = _mapper.Map<AttachmentReadDto>(attachmentModel);

            return CreatedAtRoute(nameof(GetAttachmentById), new { Id = attachmentReadDto.Id }, attachmentReadDto);
        }

        //DELETE api/attachments/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteAttachment(int id)
        {

            var attachmentModel = _onlineDOD.GetAttachmentById(id);
            if (attachmentModel == null)
            {
                return NotFound();
            }

            _onlineDOD.DeleteAttachment(attachmentModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }
    }
}
