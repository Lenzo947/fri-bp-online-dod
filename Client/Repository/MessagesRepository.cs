using BP_OnlineDOD.Client.Helpers;
using BP_OnlineDOD.Shared.DTOs;
using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly IHttpService httpService;
        private string url = "api/messages";

        public MessagesRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await httpService.GetHelper<List<Message>>(url, includeToken: false);
        }

        public async Task<List<Message>> GetAllMessagesWithDeleted()
        {
            return await httpService.GetHelper<List<Message>>(url, includeToken: true);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await httpService.GetHelper<Message>($"{url}/{id}", includeToken: false);
        }

        public async Task<HttpResponseWrapper<Message>> CreateMessage(Message message)
        {
            var response = await httpService.Post<Message, Message>(url, message, includeToken: false);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<object>> UpdateMessage(Message message)
        {
            var response = await httpService.Put($"{url}/{message.Id}", message);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<object>> HideMessage(int Id)
        {
            var response = await httpService.Delete($"{url}/hide/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<object>> RenewMessage(int Id)
        {
            var response = await httpService.Delete($"{url}/renew/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<object>> UpvoteMessage(int Id)
        {
            var response = await httpService.Post<object>($"{url}/upvote/{Id}", null);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<object>> CancelMessageUpvote(int Id)
        {
            var response = await httpService.Post<object>($"{url}/cancel-upvote/{Id}", null);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }

        public async Task<HttpResponseWrapper<Attachment>> CreateAttachment(Attachment attachment)
        {
            var response = await httpService.Post<Attachment, Attachment>("api/attachments", attachment, includeToken: false);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response;
        }
    }
}
