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

        public void CreateAttachment(Attachment a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            _context.Attachments.Add(a);
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

        public void DeleteAttachment(Attachment a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            _context.Attachments.Remove(a);
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

        public ICollection<Attachment> GetAllAttachments()
        {
            return _context.Attachments.ToList();
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
                .Include(m => m.Attachments)
                //.Where(m => m.Deleted == false)
                .ToList();

            return result;

            //return _context.Messages.ToList();
        }

        public Attachment GetAttachmentById(int id)
        {
            return _context.Attachments.FirstOrDefault(p => p.Id == id);
        }

        public BlockedIP GetBlockedIPById(int id)
        {
            return _context.BlockedIPs.FirstOrDefault(p => p.Id == id);
        }

        public Message GetMessageById(int id)
        {
            return _context.Messages
                .Include(m => m.Attachments)
                .FirstOrDefault(p => p.Id == id);
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