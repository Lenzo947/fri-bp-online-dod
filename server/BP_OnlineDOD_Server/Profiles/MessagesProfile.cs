using AutoMapper;
using BP_OnlineDOD_Server.Dtos;
using BP_OnlineDOD_Server.Models;

namespace BP_OnlineDOD_Server.Profiles
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            //Source -> Target
            CreateMap<Message, MessageReadDto>();
            CreateMap<MessageCreateDto, Message>();
            CreateMap<MessageUpdateDto, Message>();
            CreateMap<Message, MessageUpdateDto>();
        }
    }
}