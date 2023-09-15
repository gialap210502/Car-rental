using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_rental.Data;
using Car_rental.Models;
using Car_rental.Untils;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;
using MySignalRApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;



namespace Car_rental.Controllers
{
    public class UserController : Controller
    {
        private IWebHostEnvironment _hostEnvironment;
        private readonly Car_rentalContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        const string SessionName = "_Name";
        const string SessionImage = "_Image";
        const string SessionId = "_ID";
        const string SessionRole = "_Role";

        public UserController(IWebHostEnvironment hostEnvironment, Car_rentalContext context, IHubContext<ChatHub> hubContext)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _hubContext = hubContext;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            ViewBag.layout = "_AdminLayout";
            return _context.user != null ?
                        View(await _context.user.ToListAsync()) :
                        Problem("Entity set 'Car_rentalContext.user'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: User/Profile/5
        public async Task<IActionResult> Profile(int? id)
        {
            ViewBag.layout = "_Layout";
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewBag.layout = "_AdminLayout";
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Layout = "_Layout";
            return View();
        }
        //User1 is the person creating the chat, user2 is the person receiving the message
        public IActionResult CreateChatRoom(int UserId1, int UserId2)
        {
            ViewBag.Layout = "_Layout";
            var user1 = _context.user.Find(UserId1).email;
            var user2 = _context.user.Find(UserId2).email;
            // Tìm cuộc trò chuyện chứa cả hai người dùng
            var existingConversation = _context.Conversation
                .Where(c => c.Participations.Any(p => p.UserID == UserId1) && c.Participations.Any(p => p.UserID == UserId2))
                .FirstOrDefault();

            if (existingConversation == null)
            {

                // Nếu không có cuộc trò chuyện tồn tại, tạo mới một cuộc trò chuyện
                var newConversation = new Conversation { Name = user1 + "-" + user2, CreatedAt = DateTime.Now };
                _context.Conversation.Add(newConversation);
                _context.SaveChanges();

                // Thêm hai người dùng vào cuộc trò chuyện mới
                var participation1 = new Participation { UserID = UserId1, ConversationID = newConversation.ConversationID };
                var participation2 = new Participation { UserID = UserId2, ConversationID = newConversation.ConversationID };
                _context.Participation.AddRange(participation1, participation2);
                _context.SaveChanges();

                // Chuyển hướng đến trang chat với cuộc trò chuyện mới
                return RedirectToAction("Chat", new { conversationId = newConversation.ConversationID });
            }
            else
            {
                // Nếu cuộc trò chuyện đã tồn tại, chuyển hướng đến trang chat với cuộc trò chuyện đã tồn tại
                return RedirectToAction("Chat", new { conversationId = existingConversation.ConversationID });
            }
        }
        public async Task<IActionResult> Chat(int? conversationId)
        {
            ViewBag.layout = "_Layout";
            var userID = HttpContext.Session.GetInt32(SessionId);
            var userImgString = "";
            if (userID != null)
            {
                userImgString = _context.user.Find(userID).image;
            }
            ViewBag.userImg = userImgString;
            List<Conversation> ConversationData = _context.Conversation.Include(C => C.Messages).ThenInclude(m => m.user).Include(C => C.Participations).Where(C => C.ConversationID == conversationId).ToList();
            return View(ConversationData);
        }
        public async Task<IActionResult> Message()
        {
            ViewBag.layout = "_Layout";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int conversationId, int userId, string content)
        {
            ViewBag.Layout = "_Layout";

            if (ModelState.IsValid)
            {
                // Assuming you have a DbContext named _context for database operations.
                var message = new Message
                {
                    ConversationID = conversationId,
                    UserID = userId,
                    Content = content,
                    SentAt = DateTime.Now
                };

                // Add the message to the database and save changes.
                _context.Message.Add(message);
                _context.SaveChanges();

                return new EmptyResult();
                // Optionally, you can redirect the user back to the chat page or perform other actions.
            }

            // If ModelState is not valid, you can handle validation errors.
            // For example, you can return the user to the chat page with validation errors.
            return View("Chat");
        }
        public IActionResult Login()
        {
            ViewBag.Layout = "_Layout";
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile myfile, [Bind("id,name,citizen_identification,driver_license,phone,dob,email,password,flag,image")] user user)
        {
            ViewBag.layout = "_AdminLayout";
            var Encode = new Encode();
            string en_password;
            en_password = Encode.encode(user.password.ToString());
            if (ModelState.IsValid)
            {
                user.password = en_password;
                Send send = new Send();
                if (myfile == null)
                {
                    user.image = null;
                }
                else
                {
                    string filename = Path.GetFileName(myfile.FileName);
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "ImageUser");
                    string fullPath = filePath + "\\" + filename;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile.CopyToAsync(stream);
                    }
                    user.image = filename;
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                var users = _context.user.FirstOrDefault(p => p.id == user.id);
                var Role = _context.roles.Where(r => r.role == "User").FirstOrDefault();
                if (users != null)
                {
                    var userRoles = new userRole();
                    userRoles.roleId = Role.id;
                    userRoles.userId = users.id;
                    _context.Add(userRoles);
                    await _context.SaveChangesAsync();
                }
                var email = user.email.ToString();
                var subject = "PLEASE CONFIRM YOUR EMAIL BY CLICK IN LINK";
                string body = "Please click the link below to confirm your account: " + "\n\n" + "https://localhost:7160/User/ConfirmAccount?email=" + email; ;
                send.SendEmail(email, subject, body);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormFile avatar, IFormFile license, IFormFile Id, string name
        , string phone, string address, DateTime dob, string emailUser, string password, int flag, String repassword)
        {
            ViewBag.Layout = "_Layout";


            var UserExists = _context.user.FirstOrDefault(u => u.email == emailUser);

            TimeSpan time = (TimeSpan)(DateTime.Now - dob);
            int yearGap = (int)(time.TotalDays / 365.25);

            var Encode = new Encode();
            string en_password;
            string en_repassword;
            en_password = Encode.encode(password.ToString());
            en_repassword = Encode.encode(repassword);

            if (en_password != en_repassword)
            {
                ViewBag.ErrorMessage = "Repeated Password does not match with Password";
            }
            else if (yearGap < 18)
            {
                ViewBag.ErrorMessage = "Your age must be over 17 years";
            }
            else if (UserExists != null)
            {
                ViewBag.ErrorMessage = "Your Email is already in use";
            }
            else if (en_password == en_repassword)
            {
                var user = new user();

                user.address = address;
                user.name = name;
                user.dob = dob;
                user.email = emailUser;
                user.flag = 0;
                user.phone = phone;
                user.password = en_password;
                Send send = new Send();
                if (avatar == null)
                {
                    user.image = null;
                }
                else
                {
                    string filename = Path.GetFileName(avatar.FileName);
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "ImageUser");
                    string fullPath = filePath + "\\" + filename;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }
                    user.image = filename;
                }
                if (license == null)
                {
                    user.driver_license = null;
                }
                else
                {
                    string filename = Path.GetFileName(license.FileName);
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "DriveLicense");
                    string fullPath = filePath + "\\" + filename;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await license.CopyToAsync(stream);
                    }
                    user.driver_license = filename;
                }
                if (Id == null)
                {
                    user.citizen_identification = null;
                }
                else
                {
                    string filename = Path.GetFileName(Id.FileName);
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Identify");
                    string fullPath = filePath + "\\" + filename;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await Id.CopyToAsync(stream);
                    }
                    user.citizen_identification = filename;
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                var users = _context.user.FirstOrDefault(p => p.id == user.id);
                var Role = _context.roles.Where(r => r.role == "User").FirstOrDefault();
                if (users != null)
                {
                    var userRoles = new userRole();
                    userRoles.roleId = Role.id;
                    userRoles.userId = users.id;
                    _context.Add(userRoles);
                    await _context.SaveChangesAsync();
                }
                var email = user.email.ToString();
                var subject = "PLEASE CONFIRM YOUR EMAIL BY CLICK IN LINK";
                string body = "Please click the link below to confirm your account: " + "\n\n" + "https://localhost:7160/User/ConfirmAccount?email=" + email; ;
                send.SendEmail(email, subject, body);
                return RedirectToAction(nameof(Login));
            }
            return View();
        }

        public IActionResult BusinessRegister(int userId)
        {
            var user = _context.user.Find(userId);
            Send send = new Send();
            var email = user.email.ToString();
            var subject = user.name + "WANT TO REGISTER A BUSINESS ACCOUNT";
            string body = user.name + "have ID : " + user.id + "\n\n" + "want to register a business account, please check this information and call to this user phone: " + user.phone;
            send.SendEmailRegister(email, subject, body);

            var subjectForUser = "YOUR REQUEST HAVE BEEN SENT";
            string bodyForUser = "Your request have been sent."+ "\n\n" +  "We will call you soon, please check your phone";
            send.SendEmail(email, subjectForUser, bodyForUser);
            return RedirectToAction("Profile", new { id = userId });
        }
        public IActionResult Logout()
        {
            ViewBag.layout = "_AdminLayout";
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "user");
        }

        public IActionResult TermAndCondition(){
            ViewBag.Layout = "_Layout";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(String UserName, String Password)
        {
            ViewBag.Layout = "_Layout";
            var Encode = new Encode();
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                var en_password = Encode.encode(Password);
                var user = await _context.user.FirstOrDefaultAsync(u => u.email == UserName && u.password == en_password);

                if (user != null /*&& user.flag == 1*/)
                {
                    var userRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == user.id);
                    var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId);
                    HttpContext.Session.SetString(SessionName, user.name);
                    if (user.image != null)
                    {
                        HttpContext.Session.SetString(SessionImage, user.image);
                    }
                    else
                    {
                        HttpContext.Session.SetString(SessionImage, "@@@@.png");
                    }
                    HttpContext.Session.SetInt32(SessionId, user.id);
                    HttpContext.Session.SetString(SessionRole, userRole.role.role);

                    return RedirectToAction("Home", "Car");
                }
                else if (user == null)
                {
                    ViewBag.ErrorMessage = "Your username or password is incorrect !";
                }
                else if (user != null && user.flag == 0)
                {
                    ViewBag.ErrorMessage = "Your are not yet confirm your email, please Confirm Your email !";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or Password cannot be empty";
            }
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.layout = "_Layout";
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile CI, IFormFile Drive, IFormFile myfile, [Bind("id,name,phone,address,dob,email,password,flag")] user user)
        {
            ViewBag.layout = "_Layout";
            var userTempImage = _context.user.Find(id).image;
            _context.Entry(_context.user.Find(id)).State = EntityState.Detached;
            var userTempID = _context.user.Find(id).citizen_identification;
            _context.Entry(_context.user.Find(id)).State = EntityState.Detached;
            var userTempDrive = _context.user.Find(id).driver_license;
            _context.Entry(_context.user.Find(id)).State = EntityState.Detached;
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (myfile == null)
                    {
                        user.image = userTempImage;

                        _context.Update(user);
                    }
                    else
                    {
                        string filename = Path.GetFileName(myfile.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "ImageUser");
                        string fullPath = filePath + "\\" + filename;
                        // Copy files to FileSystem using Streams
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await myfile.CopyToAsync(stream);
                        }
                        user.image = filename;
                        _context.Update(user);
                    }
                    if (CI == null)
                    {
                        user.citizen_identification = userTempID;
                        _context.Update(user);
                    }
                    else
                    {
                        string filename = Path.GetFileName(CI.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Identify");
                        string fullPath = filePath + "\\" + filename;
                        // Copy files to FileSystem using Streams
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await CI.CopyToAsync(stream);
                        }
                        user.citizen_identification = filename;
                        _context.Update(user);
                    }
                    if (Drive == null)
                    {
                        user.driver_license = userTempDrive;
                        _context.Update(user);
                    }
                    else
                    {
                        string filename = Path.GetFileName(myfile.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "DriveLicense");
                        string fullPath = filePath + "\\" + filename;
                        // Copy files to FileSystem using Streams
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await myfile.CopyToAsync(stream);
                        }
                        user.driver_license = filename;
                        _context.Update(user);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(user.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", new { id = user.id });
            }
            return RedirectToAction("Profile", new { id = user.id });
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.layout = "_AdminLayout";
            if (_context.user == null)
            {
                return Problem("Entity set 'Car_rentalContext.user'  is null.");
            }
            var user = await _context.user.FindAsync(id);
            if (user != null)
            {
                _context.user.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmAccount(String email)
        {
            ViewBag.layout = "_AdminLayout";
            var user = await _context.user.FirstOrDefaultAsync(u => u.email == email);
            string message = "";
            if (user != null && user.flag == 0)
            {
                user.flag = 1;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else if (user == null)
            {
                message = "Your email does not create";
            }
            ViewBag.message = message;
            return View();
        }

        private bool userExists(int id)
        {
            ViewBag.layout = "_AdminLayout";
            return (_context.user?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
