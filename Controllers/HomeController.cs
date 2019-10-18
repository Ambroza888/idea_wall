using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CS_proj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CS_proj.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        // ---------------------------------------------------------------------
        // Index
        // ---------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("index");
        }
        // ---------------------------------------------------------------------
        // Registrating to db
        // ---------------------------------------------------------------------
        [HttpPost("/Register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email","Email already in use!");
                    return View("Index");
                }
                else if(dbContext.Users.Any(u => u.FirstName == user.FirstName))
                {
                    ModelState.AddModelError("FirstName","First Name is already in use!");
                    return View("Index");
                }
                else if(dbContext.Users.Any(u => u.LastName == user.LastName))
                {
                    ModelState.AddModelError("LastName","Last Name is already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user,user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("user_id", user.UserId);
                return RedirectToAction("Dashbord");
            }
            return View("Index");
        }
        // ---------------------------------------------------------------------
        // Login Page
        // ---------------------------------------------------------------------
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }
        // ---------------------------------------------------------------------
        // Log in TO success or BELT EXAM would be this page !
        // ---------------------------------------------------------------------
        [HttpPost("logToSuccess")]
        public IActionResult LoginSuccess(LoginUser userSubmision)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmision.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email","Invalid Email/Password");
                    return View("Login");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmision,userInDb.Password,userSubmision.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password","Invalid Email/Password");
                    return View("Login");
                }
                //--------------------- Creating session after all those checks -------------------
                int user_id = userInDb.UserId;
                HttpContext.Session.SetInt32("user_id",user_id);
                return RedirectToAction("Dashbord");
            }
            return View("Login");
        }
        // ---------------------------------------------------------------------
        // Clear session
        // ---------------------------------------------------------------------
        [HttpGet("clear")]
        public IActionResult clearSession()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        // ---------------------------------------------------------------------
        // Success PAGE potential belt-exam page
        // ---------------------------------------------------------------------

        [HttpGet("/Dashbord")]
        public IActionResult Dashbord()
        {
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Login");
            }
            
            User User = dbContext.Users
            .Include(u =>u.Ideas)
                .ThenInclude(u =>u.Likes)
                    .Include(u =>u.Likes)
                        .ThenInclude(u =>u.Idea)
                            .FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("user_id"));
            List<Idea> all_ideas = dbContext.Ideas
                .Include(u => u.User)
                    .Include(u => u.Likes)
                        .ThenInclude(u => u.User)
                            .OrderByDescending(w=>w.Likes.Count)
                            .ToList();
            ViewBag.all_ideas = all_ideas;
            ViewBag.User = User;

                return View("Dashbord");
        }

        // ---------------------------------------------------------------------
        // Create Idea
        // ---------------------------------------------------------------------

        [HttpPost("/createIdea")]
        public IActionResult createIdea(Idea newidea)
        {
            if(HttpContext.Session.GetInt32("user_id")==null)
            {
                return RedirectToAction("Login");
            }
            if(ModelState.IsValid)
            {
                dbContext.Add(newidea);
                dbContext.SaveChanges();
            return RedirectToAction("Dashbord");
            }
            else
            {

            User User = dbContext.Users
                .Include(u =>u.Ideas)
                    .ThenInclude(u =>u.Likes)
                        .Include(u =>u.Likes)
                            .ThenInclude(u =>u.Idea)
                                .FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("user_id"));
            List<Idea> all_ideas = dbContext.Ideas
                .Include(u => u.User)
                    .Include(u => u.Likes)
                        .ThenInclude(u => u.User)
                            .OrderByDescending(w=>w.Likes.Count)
                            .ToList();
            ViewBag.all_ideas = all_ideas;
            ViewBag.User = User;
                return View("Dashbord");
            }
        }


        // ---------------------------------------------------------------------
        // Add a like
        // ---------------------------------------------------------------------
        [HttpGet("addLike/{IdeaId}")]
        public IActionResult addLike(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("user_id")== null)
            {
                return RedirectToAction("Login"); 
            }
            User User = dbContext.Users
            .Include(u =>u.Likes)
                .ThenInclude(u => u.Idea)
                    .Include(u =>u.Ideas)
                        .ThenInclude(u => u.Likes)
                            .FirstOrDefault(u =>u.UserId == HttpContext.Session.GetInt32("user_id"));
            Idea Idea = dbContext.Ideas
                .Include(u=>u.User)
                    .Include(u => u.Likes)
                        .ThenInclude(u=>u.User)
                            .FirstOrDefault(u =>u.IdeaId == IdeaId);
            Like newLike = new Like
            {
                User = User,
                Idea = Idea,
                IdeaId = IdeaId,
                UserId = User.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            dbContext.Add(newLike);
            dbContext.SaveChanges();
            return RedirectToAction("Dashbord");
        }

        // ---------------------------------------------------------------------
        // DELETE IDEA
        // ---------------------------------------------------------------------
        [HttpGet("/Delete/{IdeaId}")]
        public IActionResult DeleteIdea(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("user_id")==null)
            {
                return RedirectToAction("Login");
            }
            Idea idea = dbContext.Ideas.FirstOrDefault(q =>q.IdeaId == IdeaId);
            dbContext.Remove(idea);
            dbContext.SaveChanges();
            return RedirectToAction("Dashbord");
        }

        // ---------------------------------------------------------------------
        // Un-like Idea
        // ---------------------------------------------------------------------
        [HttpGet("/Unlike/{IdeaId}")]
        public IActionResult UnlikeIdea(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("user_id")==null)
            {
                return RedirectToAction("Login");
            }
            int user_id = (int)HttpContext.Session.GetInt32("user_id");
            Like like = dbContext.Likes.FirstOrDefault(u =>u.UserId == user_id && u.IdeaId == IdeaId);
            dbContext.Remove(like);
            dbContext.SaveChanges();
            return RedirectToAction("Dashbord");
        }

        [HttpGet("/IdeaINFO/{IdeaId}")]
        public IActionResult IdeaINFO(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("user_id")==null)
            {
                return RedirectToAction("Login");
            }
            Idea oneIdea = dbContext.Ideas
                .Include(u =>u.User)
                    .Include(l => l.Likes)
                        .ThenInclude(l => l.User)
                            .FirstOrDefault(l =>l.IdeaId == IdeaId);
            ViewBag.oneIdea = oneIdea;
            return View("IdeaINFO");

        }


        // ---------------------------------------------------------------------
        // UserINFO
        // ---------------------------------------------------------------------
        [HttpGet("/UserINFO/{UserId}")]
        public IActionResult UserINFO(int UserId)
        {
            if(HttpContext.Session.GetInt32("user_id")==null)
            {
                return RedirectToAction("Login");
            }
            User User = dbContext.Users
                .Include(l => l.Ideas)
                    .ThenInclude(l => l.User)
                        .Include(l=>l.Likes)
                            .ThenInclude(l => l.Idea)
                                .FirstOrDefault(l => l.UserId == UserId);

            List<Idea> all_ideas = dbContext.Ideas
                .Include(w => w.User)
                    .Include(l =>l.Likes)
                        .ThenInclude(l => l.User)
                            .ToList();

            List<Like> all_likes = dbContext.Likes
                .Include(w => w.User)
                    .Include(w =>w.Idea)
                        .ToList();
            List<Like> notlike = new List<Like>();
            foreach(var i in all_likes)
            {
                if(i.UserId != User.UserId)
                {
                    notlike.Add(i);
                }
            }

            ViewBag.notlike = notlike;
            ViewBag.User = User;
            return View("UserINFO");
        }

















        // ---------------------------------------------------------------------
        // ERRORRR
        // ---------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
