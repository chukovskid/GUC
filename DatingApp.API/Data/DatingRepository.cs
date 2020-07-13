using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;


namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }




        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.User.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.User.Include(p => p.Photos).AsQueryable(); // mora .AsQueryble da go stavam za da ne javuva error dole
            users = users.OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Gender == userParams.Gender); // gi Trgam site site od ist Pol (ostanuvaat site so Razlicen)// 145// 
            users = users.Where(u => u.Id != userParams.UserId);

            if (userParams.Likers)
            {
                var userLikees = await GerUserLikes(userParams.UserId, userParams.Likers); // so ova gi imam site sto ME lajknale(lista od nivnite ID)
                users = users.Where(u => userLikees.Contains(u.Id));
            }
            if (userParams.Likees)
            { // zavisno koe e True od dvete
                var userLikers = await GerUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));  /// zemi gi USERITE so pomos na nivnite Ids
            }


            if (userParams.maxAge < 99 || userParams.minAge > 18)
            {
                var maxDob = DateTime.Today.AddYears(-userParams.minAge - 1);
                var minDob = DateTime.Today.AddYears(-userParams.maxAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }
            if (userParams.orderBy != null)
            {
                switch (userParams.orderBy)
                {
                    case "created": // od SPA prakjam lastActive i created. ako ne klikne da specifira deka e created po default ke teram lastActive
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<int>> GerUserLikes(int id, bool likers) // 155
        {
            var user = await _context.User
            .Include(l => l.Likers)
            .Include(l => l.Likees)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                // site Likers vo Userot koi imaat lajknato user so moe ID. daj mi go nivniot Id
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }

        }

        public async Task<Photo> GetPhoto(int id) // 108
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(u => u.Id == id);
            return photo;

        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var photo = await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
            return photo;
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(u =>
                u.LikerId == userId && u.LikeeId == recipientId);
            return like;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams) // Outbox, Inbox, Unread
        {
            var messages = _context.Messages
            .Include(s => s.Sender).ThenInclude(p => p.Photos)
            .Include(r => r.Recipient).ThenInclude(p => p.Photos)
            .AsQueryable();


            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDelete == false);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                    break;

                default:
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDelete == false && m.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
             var messages = await _context.Messages
            .Include(s => s.Sender).ThenInclude(p => p.Photos)
            .Include(r => r.Recipient).ThenInclude(p => p.Photos)
            .Where(s => s.SenderId == userId && s.SenderDeleted == false && s.RecipientId == recipientId
            || s.SenderId == recipientId && s.RecipientDelete == false && s.RecipientId == userId)
            .OrderByDescending(d => d.MessageSent)
            .ToListAsync();

            return messages;
        }
    }
}