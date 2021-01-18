using System.Collections;
using System.Collections.Generic;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Server.Data
{
    public interface IOnlineDOD
    {
        bool SaveChanges();

        ICollection<Log> GetAllLogs();

        ICollection<BlockedIP> GetAllBlockedIPs();

        BlockedIP GetBlockedIPById(int id);

        void CreateBlockedIP(BlockedIP ip);

        void DeleteBlockedIP(BlockedIP ip);

        ICollection<Message> GetAllMessages();

        Message GetMessageById(int id);

        void CreateMessage(Message msg);

        void UpdateMessage(Message msg);

        void DeleteMessage(Message msg);

    }
}
