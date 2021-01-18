using BP_OnlineDOD.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BP_OnlineDOD.Server.Data
{
    public class SqlOnlineDOD : IOnlineDOD
    {
        private readonly OnlineDODContext _context;

        public SqlOnlineDOD(OnlineDODContext context)
        {
            _context = context;
        }
        public void CreateBlockedIP(BlockedIP ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            _context.BlockedIPs.Add(ip);
        }

        public void CreateMessage(Message msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            _context.Messages.Add(msg);
        }

        public void DeleteBlockedIP(BlockedIP ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            _context.BlockedIPs.Remove(ip);
        }

        public void DeleteMessage(Message msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            _context.Messages.Remove(msg);
        }

        public ICollection<BlockedIP> GetAllBlockedIPs()
        {
            return _context.BlockedIPs.ToList();
        }

        public ICollection<Log> GetAllLogs()
        {
            return _context.Logs.ToList();
        }

        public ICollection<Message> GetAllMessages()
        {
            var result = _context
                .Messages
                .Include(m => m.ChildMessages)
                .ToList();

            return result
                .Where(e => e.ParentMessageId == null)
                .ToList();

            //return _context.Messages.ToList();
        }

        public BlockedIP GetBlockedIPById(int id)
        {
            return _context.BlockedIPs.FirstOrDefault(p => p.Id == id);
        }

        public Message GetMessageById(int id)
        {
            return _context.Messages.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateMessage(Message msg)
        {
            //nothing
        }
    }
}