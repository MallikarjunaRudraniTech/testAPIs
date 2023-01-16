﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using testAPIs.Models;

namespace testAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TestDbContext _context;

        public LoginController(TestDbContext context)
        {
            _context = context;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/Login/5
        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<UserTable>> GetUserTable(string email,string password)
        {
            var userDetails=new UserTable();
            //var userDetailsList=new List<UserTable>();
            //userDetailsList= await _context.UserTables.ToListAsync();

            //Console.WriteLine(userDetails+"from 38");
            var userTable = await _context.UserTables.Where(x =>  x.Email == email && x.Password == password).FirstOrDefaultAsync();
            /*var userTable1 = await _context.UserTables.FindBestMatch(id);*/

            if (userTable != null && userTable.Password==password)
            {
                //userDetails.Id = id;
                userDetails.FirstName= userTable.FirstName;
                userDetails.LastName = userTable.LastName;
                userDetails.PhoneNumber = userTable.PhoneNumber;
                userDetails.Email = userTable.Email;
                userDetails.AadharNumber= userTable.AadharNumber;
                userDetails.Passport= userTable.Passport;

                return userDetails;   
            }

            return NotFound();
        }

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UserTable userTable)
        {
            if (id != userTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUserTable(LogInReqBody logInReqBody)
        {
            var userDetails = new UserTable();
            
            var userTable = await _context.UserTables.Where(x => x.Email == logInReqBody.email && x.Password == logInReqBody.password).FirstOrDefaultAsync();

            if (userTable != null)
            {
                //userDetails.Id = id;
                userDetails.FirstName = userTable.FirstName;
                userDetails.LastName = userTable.LastName;
                userDetails.PhoneNumber = userTable.PhoneNumber;
                userDetails.Email = userTable.Email;
                userDetails.AadharNumber = userTable.AadharNumber;
                userDetails.Passport = userTable.Passport;

                return userDetails;
            }

            return NotFound();
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return _context.UserTables.Any(e => e.Id == id);
        }
    }

    

}
