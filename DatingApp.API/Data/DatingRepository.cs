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
            users = users.Where(u => u.Id != userParams.UserId); // i da ne go dava LOGIRANIOT USER
                                                                 // dodavam filtriranje spored AGE
                                                                 // napravi max i min promenlivi i ako e pogolemo ili pomalo od dadenite 
            if (userParams.maxAge < 99 || userParams.minAge > 18)
            {
                var maxDob = DateTime.Today.AddYears(-userParams.minAge - 1);
                var minDob = DateTime.Today.AddYears(-userParams.maxAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }
            if(userParams.orderBy != null){
                    switch (userParams.orderBy) {
                    case"created": // od SPA prakjam lastActive i created. ako ne klikne da specifira deka e created po default ke teram lastActive
                    users = users.OrderByDescending(u => u.Created);
                     break;
                     default:
                    users = users.OrderByDescending(u => u.LastActive);
                    break;
                }                
            }



            // sakam da mi vrati: useri izretceni od fukcijata vo PagedList.CreateAsync
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
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
    }
}