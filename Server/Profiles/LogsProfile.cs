using AutoMapper;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Server.Profiles
{
    public class LogsProfile : Profile
    {
        public LogsProfile()
        {
            //Source -> Target
            CreateMap<Log, LogReadDto>();
        }
    }
}
