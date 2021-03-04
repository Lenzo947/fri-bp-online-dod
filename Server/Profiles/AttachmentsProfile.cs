using AutoMapper;
using BP_OnlineDOD.Server.Dtos;
using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Profiles
{
    public class AttachmentsProfile : Profile
    {
        public AttachmentsProfile()
        {
            //Source -> Target
            CreateMap<Attachment, AttachmentReadDto>();
            CreateMap<AttachmentCreateDto, Attachment>();
        }
    }
}
