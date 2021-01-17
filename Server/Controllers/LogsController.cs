using AutoMapper;
using BP_OnlineDOD.Server.Data;
using BP_OnlineDOD.Server.Dtos;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BP_OnlineDOD.Server.Controllers
{

    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;
        private readonly IMapper _mapper;

        public LogsController(IOnlineDOD onlineDOD, IMapper mapper)
        {
            _onlineDOD = onlineDOD;
            _mapper = mapper;
        }

        //GET api/messages
        [HttpGet]
        public ActionResult<IEnumerable<LogReadDto>> GetAllLogs()
        {
            //Serilog.Log.Information("GET /api/logs");

            var logItems = _onlineDOD.GetAllLogs();

            return Ok(_mapper.Map<IEnumerable<LogReadDto>>(logItems));
        }
    }
}
