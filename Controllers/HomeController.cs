using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using logRegDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace logRegDemo.Controllers
{
    public class HomeController : Controller
    {
        private UserContext dbContext;

        public HomeController(UserContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Success")]
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User NewUser)
        {
            NewUser.CreatedAt = DateTime.Now;
            NewUser.UpdatedAt = DateTime.Now;
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.users.Any(u => u.Email == NewUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    return RedirectToAction("Index");
                    // You may consider returning to the View at this point
                }
                //LOGIC FOR ADDING TO DB
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                dbContext.Add(NewUser);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            return RedirectToAction("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(string LoginEmail, string LoginPass)
        {
            //Check if exists by querying db using email
            User DatabaseUser = dbContext.users.SingleOrDefault(u=>u.Email == LoginEmail);
            if(DatabaseUser == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                 var hasher = new PasswordHasher<User>();
            
                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(DatabaseUser, DatabaseUser.Password, LoginPass);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    System.Console.WriteLine("Didn't match pass");
                    return RedirectToAction("Index");
                    // handle failure (this should be similar to how "existing email" is handled)
                }
                else
                {
                    System.Console.WriteLine("Did match pass");
                    return RedirectToAction("Success");
                }
            }
        }
    }

}
