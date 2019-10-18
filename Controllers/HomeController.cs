using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {

        private MyContext db;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            db = context;
        }
     
        [HttpGet("")]
        public IActionResult Register()
        {
            return View("Registration");
        }

        [HttpPost("home/CreateRegister")]
        public IActionResult CreateRegister(User newUser)
        {

            if (ModelState.IsValid)
            {
                bool isEmailTaken =
                    db.Users.Any(user => newUser.Email == user.Email);

                if (isEmailTaken)
                {
                    ModelState.AddModelError("Email", "Email Taken");
                }
            }

            if (ModelState.IsValid == false)
            {
                return View("Registration");
            }
            // No Errors
            // Hash pw
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("success");
        }

        [HttpPost("home/loggingIN")]
        public IActionResult loggingIN(LoginUser loginUser)
        {
            string genericError = "Invalid Credentials";

            if (ModelState.IsValid == false)
            {
                return View("Registration");
            }
            User dbUser = db.Users.FirstOrDefault(user => loginUser.LoginEmail == user.Email);
            if (dbUser == null)
            {
                ModelState.AddModelError("LoginEmail", genericError);
                return View("Registration");
            }
            // dbUser is not null
            // Convert LoginUser to User for PasswordVerification
            User viewUser = new User
            {
                Email = loginUser.LoginEmail,
                Password = loginUser.LoginPassword
            };
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            PasswordVerificationResult passwordCheck =
                hasher.VerifyHashedPassword(viewUser, dbUser.Password, viewUser.Password);
            // failed pw match
            if (passwordCheck == 0)
            {
                ModelState.AddModelError("LoginEmail", genericError);
                return View("Registration");
            }
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            return RedirectToAction("success");
        }
        [HttpGet("home/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }
   
   
        [HttpGet("success")]
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("UserId") == null){
            return Register();
            }
            else{
            ViewBag.allTheWeddings = db.Wedding
            .Include (p =>p.Creator)
            .Include (p =>p.Attendees).ToList();
            ViewBag.UID =HttpContext.Session.GetInt32("UserId");

            ViewBag.TheGuestList = db.Guest
            .Include (p => p.Attendees).ToList();
    

            ViewBag.TheGuestList2 = db.Users
            .Include (p => p.Events).ToList();
            

            return View();
            }
        }

///////////////////////////////////////////////////

        [HttpGet("home/AddNewWedding")]
        public IActionResult AddNewWedding()
        {
                        if (HttpContext.Session.GetInt32("UserId") == null){
            return Register();
            }
            else{
            ViewBag.Uid = HttpContext.Session.GetInt32("UserId");
            return View();
            }
        }

        [HttpPost("home/AddingWedding")]
        public IActionResult AddingWedding(Wedding NewWedding)
        {
            int ? Uid = HttpContext.Session.GetInt32("UserId");
             NewWedding.Creator = db.Users.FirstOrDefault(p => p.UserId == Uid);
            db.Add(NewWedding);
            db.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpGet("home/WeddingDetail/{id}")]
        public IActionResult WeddingDetail(int id)
        {
                        if (HttpContext.Session.GetInt32("UserId") == null){
            return Register();
            }
            else{
            ViewBag.WeddingDetailsPageInfo = db.Wedding
            .Include(Wedding =>Wedding.Creator)
            .Include(wed => wed.Attendees)
            .ThenInclude(p=>p.Attendees)
            .FirstOrDefault(W =>W.WeddingId == id);



            // ViewBag.WeddingDetailsGuestList = db.Guest
            // .Include(wed => wed.Attendees)
            // .ThenInclude(p=>p.UserId)
            // .FirstOrDefault(W =>W.WeddingId == id);


            return View();
        }
        }

        [HttpGet("home/DeleteWedding/{id}")]
        public IActionResult DeleteWedding(int id)
        {
            Wedding thisWedding = db.Wedding
            .Include(Wedding =>Wedding.Creator)
            .FirstOrDefault(W =>W.WeddingId == id);
            db.Remove(thisWedding);
            db.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpGet("home/RSVP/{id}")]
        public IActionResult RSVP(int id)
        {
            int ? Uid = HttpContext.Session.GetInt32("UserId");
            Guest NewGuest = new Guest();
            NewGuest.UserId = (int)Uid;
            NewGuest.WeddingId = id;
            db.Add(NewGuest);
            db.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpGet("home/UNRSVP/{id}")]
        public IActionResult UNRSVP(int id)
        {
            Guest thisGuest = db.Guest
            .FirstOrDefault(G =>G.GuestId == id);
            db.Remove(thisGuest);
            db.SaveChanges();
            return RedirectToAction("Success");
        }
        
    }
}
