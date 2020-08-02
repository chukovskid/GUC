using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entit) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams); // PagedList e zamena za IEnumerable<T>
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id); // S11
        Task<Photo> GetMainPhotoForUser(int id); // 113
        Task<Like> GetLike(int userId, int recipientId); // 154
        Task<Message> GetMessage(int id); // 161
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams); // 161
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId); // 161
        Task<Room> GetRoom(int Id);
        Task<IEnumerable<Room>> GetRooms();
        Task<RoomPhoto> GetRoomPhoto(int id);
         Task<Notifications> GetNotification(int notificationId);
        Task<IEnumerable<Notifications>> GetNotifications();
         Task<NotificationUser> GetNotificationUser(int userid, int notificationId);
         Task<IEnumerable<User>> GetFullUsers();



    }
}