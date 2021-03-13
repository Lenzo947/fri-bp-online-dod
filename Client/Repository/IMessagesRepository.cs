using BP_OnlineDOD.Client.Helpers;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public interface IMessagesRepository
    {
        Task<List<Message>> GetAllMessages();
        Task<Message> GetMessage(int id);
        Task<HttpResponseWrapper<Message>> CreateMessage(Message message);
        Task<HttpResponseWrapper<object>> UpdateMessage(Message message);
        Task<List<Message>> GetAllMessagesWithDeleted();
        Task<HttpResponseWrapper<object>> HideMessage(int Id);
        Task<HttpResponseWrapper<object>> RenewMessage(int Id);
        Task<HttpResponseWrapper<object>> UpvoteMessage(int Id);
        Task<HttpResponseWrapper<object>> CancelMessageUpvote(int Id);
        Task<HttpResponseWrapper<Attachment>> CreateAttachment(Attachment attachment);
    }
}
