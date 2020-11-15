using System.Collections;
using System.Collections.Generic;
using BP_OnlineDOD_Server.Models;

namespace BP_OnlineDOD_Server.Data
{
    public interface IOnlineDOD
    {
        bool SaveChanges();


        IEnumerable<Message> GetAllMessages();

        Message GetMessageById(int id);

        void CreateMessage(Message msg);

        void UpdateMessage(Message msg);

        void DeleteMessage(Message msg);

    }
}
