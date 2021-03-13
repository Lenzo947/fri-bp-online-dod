using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD.Server.Controllers
{
    [ApiController]
    [Route("api/blocked-ips")]
    [Authorize(Roles = "Admin")]
    public class BlockedIPsController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;
        private readonly IMapper _mapper;

        public BlockedIPsController(IOnlineDOD onlineDOD, IMapper mapper)
        {
            _onlineDOD = onlineDOD;
            _mapper = mapper;
        }

        //GET api/blocked-ips
        [HttpGet]
        public ActionResult<IEnumerable<BlockedIPReadDto>> GetAllBlockedIPs()
        {
            //Serilog.Log.Information("GET /api/blocked-ips");

            var blockedIP_Items = _onlineDOD.GetAllBlockedIPs();

            return Ok(_mapper.Map<IEnumerable<BlockedIPReadDto>>(blockedIP_Items));
        }

        //GET api/blocked-ips/{id}
        [HttpGet("{id}", Name = "GetBlockedIPById")]
        public ActionResult<BlockedIPReadDto> GetBlockedIPById(int id)
        {
            //Serilog.Log.Information($"GET /api/blocked-ips/{id}");

            var blockedIP_Item = _onlineDOD.GetBlockedIPById(id);
            if (blockedIP_Item != null)
            {
                return Ok(_mapper.Map<BlockedIPReadDto>(blockedIP_Item));
            }

            return NotFound();
        }

        //POST api/blocked-ips
        [HttpPost]
        public ActionResult<BlockedIPReadDto> CreateBlockedIP(BlockedIPCreateDto blockedIPCreateDto)
        {
            var blockedIPModel = _mapper.Map<BlockedIP>(blockedIPCreateDto);
            _onlineDOD.CreateBlockedIP(blockedIPModel);
            _onlineDOD.SaveChanges();

            var blockedIPReadDto = _mapper.Map<BlockedIPReadDto>(blockedIPModel);

            Serilog.Log.Information($"[{this.Request.Host.Host}] POST /api/blocked-ips -> ID - {blockedIPReadDto.Id}");

            return CreatedAtRoute(nameof(GetBlockedIPById), new { Id = blockedIPReadDto.Id }, blockedIPReadDto);
        }

        //DELETE api/blocked-ips/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteBlockedIP(int id)
        {

            var blockedIPModel = _onlineDOD.GetBlockedIPById(id);
            if (blockedIPModel == null)
            {
                return NotFound();
            }

            _onlineDOD.DeleteBlockedIP(blockedIPModel);
            _onlineDOD.SaveChanges();

            Serilog.Log.Information($"[{this.Request.Host.Host}] DELETE /api/blocked-ips/{id}");

            return NoContent();
        }
    }
}
