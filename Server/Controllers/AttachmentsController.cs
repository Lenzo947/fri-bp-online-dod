using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD.Server.Controllers
{
    [ApiController]
    [Route("api/attachments")]
    [Authorize(Roles = "Admin, Editor")]
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
        [AllowAnonymous]
        public ActionResult<IEnumerable<Attachment>> GetAllAttachments()
        {
            var attachmentItems = _onlineDOD.GetAllAttachments();

            return Ok(attachmentItems);
        }

        //GET api/attachments/{id}
        [HttpGet("{id}", Name = "GetAttachmentById")]
        [AllowAnonymous]
        public ActionResult<Attachment> GetAttachmentById(int id)
        {
            var attachmentItem = _onlineDOD.GetAttachmentById(id);
            if (attachmentItem != null)
            {
                return Ok(attachmentItem);
            }

            return NotFound();
        }

        //POST api/attachments
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<Attachment> CreateAttachment(AttachmentCreateDto attachmentCreateDto)
        {
            var attachmentModel = _mapper.Map<Attachment>(attachmentCreateDto);
            _onlineDOD.CreateAttachment(attachmentModel);
            _onlineDOD.SaveChanges();

            return CreatedAtRoute(nameof(GetAttachmentById), new { Id = attachmentModel.Id }, attachmentModel);
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
