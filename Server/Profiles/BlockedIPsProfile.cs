using AutoMapper;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Server.Profiles
{
    public class BlockedIPsProfile : Profile
    {
        public BlockedIPsProfile()
        {
            //Source -> Target
            CreateMap<BlockedIP, BlockedIPReadDto>();
            CreateMap<BlockedIPCreateDto, BlockedIP>();
        }
    }
}
