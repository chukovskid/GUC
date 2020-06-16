using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } // vkupno useri

        public PagedList(List<T> items, int count,int pageNumber, int pageSize){

            TotalCount = count; // vkupno Useri ili <T>
            PageSize = pageSize; // kolku ima Useri na po strana
            CurrentPage = pageNumber;
            // spored toa kolku useri imas (14) i po kolku sakas na po strana (5) [14/5 = 2.9] znaci barem 3 strani 
            TotalPages = (int)Math.Ceiling(count/(double)pageSize); 
            this.AddRange(items);
        }    
        // sega za da ja napolnam PageList instanca od istata 
        // vo nejze sakam da ima Skip
        public  static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize){ // 141
            var count = await source.CountAsync();
            // Skip(([2]-1)*5) == 5. ke gi skokne prvite 5 i posle so Take(5) ke gi zeme narednite 5
            var items = await source.Skip((pageNumber - 1)*pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T> (items, count, pageNumber, pageSize);   
        }


    }
}