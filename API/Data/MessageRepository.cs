using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data

{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public  void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);

        }

        public async Task<Message> GetMessage(int Id)
        {
            return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m => m.DateSent).ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .AsQueryable();
  
  
            query = messageParams.Container switch

            {
                "Inbox" => query.Where(u => u.RecipientUserName == messageParams.Username && u.RecipientDelete == false),
                "Outbox" => query.Where(u => u.SenderUserName == messageParams.Username && u.SenderDelete == false),
                _ => query.Where(u => u.RecipientUserName == messageParams.Username && u.RecipientDelete == false && u.DateRead == null)
            };

            return await PagedList<MessageDto>.CreateAsync(query, messageParams.pageNumber, messageParams.PageSize);
        }

        // public async Task<IEnumerable<MessageDto>> GetMessageThred(string currentUserName, string recipientUserName)
        // {
        //     var messages = await _context.Messages
        //     .Include(u => u.Sender).ThenInclude(p => p.Photos)
        //     .Include(u => u.Recipient).ThenInclude(p => p.Photos)
        //     .Where(m => m.Recipient.UserName == currentUserName
        //     && m.Sender.UserName == recipientUserName && m.RecipientDelete == false
        //     || m.Recipient.UserName == recipientUserName
        //     && m.Sender.UserName == currentUserName && m.SenderDelete == false)
        //     .OrderBy(m => m.DateSent)
        //     .ToListAsync();

        //     var unreadMessages = messages.Where(m => m.DateRead == null
        //     && m.Recipient.UserName == currentUserName).ToList();
        //     if (unreadMessages.Any())
        //     {
        //         foreach (var message in unreadMessages)
        //         {
        //             message.DateRead = DateTime.Now;
        //         }
        //         await _context.SaveChangesAsync();
        //     }
        //     return _mapper.Map<IEnumerable<MessageDto>>(messages);
        // }


 public async Task<IEnumerable<MessageDto>> GetMessageThred(string currentUsername,
            string recipientUsername)
        {
            var messages = await _context.Messages
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDelete == false
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername && m.SenderDelete == false
                )
                .MarkUnreadAsRead(currentUsername)
                .OrderBy(m => m.DateSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return messages;
        }
          public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }
         public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
            public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }
          public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}