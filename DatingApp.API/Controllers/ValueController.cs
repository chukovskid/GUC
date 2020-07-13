using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context; // ova e za DB koj so ke gi ima vrednostite
        public ValuesController(DataContext context) // ova e za DB gi STAVA vrednostite i e Constructor
        {
            _context = context;

        }
        // GET api/values // so ova gi vrakjam samo
        [HttpGet]
        public async Task<IActionResult> GetValues() /// So IActionResult mozeme HTPP responces da vrakjame, pr Okej sto ke vrati 200 responce, mislam ova e za podobar responce
        {// ActionResult you can return only predefined ones for returning a View or a resource. With IActionResult we can return a response, or error as well.
            // sega ke go napravam asihroniziran bidejki e podobar, ima funkcii, deelenje tabeli, filtriranje i slicno, Toa moze SAMO so asinhroni
            // sinhroniziran CODE.. sto znaci toa? deka dodeka ne gi vrati  vrednosta od DB ke stopira tuka
            var values = await _context.Values.ToListAsync(); // od DataContext go zimam Values (vo vid na lista) i go davam na var values za return
            return Ok(values); // HC vrednosti // sega od DB KE VADIMEE bez okej ke bide Null i toa e error 200
        }

        // GET api/values/5 
        [HttpGet("{id}")] // sto zima od link   
        public async Task<IActionResult> GetValue(int id) // so ova valjda go zimam ID od link
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id); // proveri go sekoe x(value) i ako e isto kako Id stavi go u value
            return Ok(value); // spored dadeno Id vrakja HC "value"
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}