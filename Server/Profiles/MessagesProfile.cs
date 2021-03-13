using AutoMapper;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Server.Profiles
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            //Source -> Target
            CreateMap<MessageCreateDto, Message>();
            CreateMap<MessageUpdateDto, Message>();
            CreateMap<Message, MessageUpdateDto>();
        }
    }
}