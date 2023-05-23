using BookStore.Common;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IConfiguration configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            this.configuration = configuration;
        }

        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(UserRegistrationModel user)
        {
            var isExist = _userService.findMember(user.Email);


            if (isExist == null)
            {
                if (ModelState.IsValid)
                {
                    string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    var encryptmail = Base64.Base64Encode(user.Email);
                    var activationUrl = $"{baseUrl}/User/ValidateUser?code.{encryptmail}.{user.FirstName} {user.LastName}";
                    var email = user.Email;

                    string logoUrl = "https://img.freepik.com/free-vector/hand-drawn-flat-design-bookstore-logo-template_23-2149337115.jpg?w=740&t=st=1679076427~exp=1679077027~hmac=80e7c2b5fed46e9a0c585577524fe30f5b13981ed6e853c092280748b9e5216f";
                    string body = $@"<!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <title>Verify Your Bookstore Account</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    font-size: 14px;
                                    line-height: 1.5;
                                    color: #333;
                                }}
                                h1, h2, h3, h4, h5, h6 {{
                                    margin-top: 0;
                                    margin-bottom: 10px;
                                }}
                                p {{
                                    margin-top: 0;
                                    margin-bottom: 10px;
                                }}
                                a {{
                                    color: #007bff;
                                    text-decoration: none;
                                }}
                                a:hover {{
                                    color: #0056b3;
                                    text-decoration: underline;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: 0 auto;
                                    padding: 20px;
                                    border: 1px solid #ccc;
                                    border-radius: 5px;
                                }}
                                .logo {{
                                    text-align: center;
                                    margin-bottom: 20px;
                                }}
                                .logo img {{
                                    max-width: 100%;
                                    height: auto;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class=""container"">
                                <div class=""logo"">
                                    <img src=""{logoUrl}"" alt=""Bookstore"">
                                </div>
                                <h1>Verify Your Bookstore Account</h1>
                                <p>Dear Mr./Ms. {user.FirstName} {user.LastName},</p>
                                <p>You have received a request to verify your account for Bookstore. To continue verification, please click on the link below:</p>
                                <p><a href=""{activationUrl}"">Verify My Account</a></p>
                                <p>If you did not request this verification, please ignore this email.</p>
                                <p>Best regards,</p>
                                <p>The Bookstore Team</p>
                            </div>
                        </body>
                        </html>";

                    var mailSender = new MailSender(configuration);
                    string value = mailSender.Send(user.Email, configuration["Yandex:Username"], "TalioBook user account verification", body);
                    if (value == "1")
                    {
                        ViewBag.SuccessMessage = 1;
                        ViewBag.AlreadyRegistered = 1;

                    }
                    var encryptPassword = Base64.Base64Encode(user.Password);
                    var encryptConfirmPassword = Base64.Base64Encode(user.ConfirmedPassword);
                    var loginModel = new LoginModel
                    {
                        Email = user.Email,
                        Password = encryptPassword,

                    };
                    var userProfileModel = new UserProfileModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,

                    };
                    user.Created = DateTime.Now;

                    user.Password = encryptPassword;
                    user.ConfirmedPassword = encryptConfirmPassword;
                    _userService.RegisterUser(user, loginModel, userProfileModel);

                    return View();

                }
            }

            ViewBag.AlreadyRegistered = -1;

            return View();
        }
        [HttpGet]
        public ActionResult ValidateUser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ValidateUser(String email)
        {
            var undefineString = "undefined";

            if (String.Equals(email, undefineString))
            {
                return RedirectToAction("Index", "Home");
            }

            var decryptmail = Base64.Base64Decode(email);
            var user = _userService.findMember(decryptmail);

            if (!user.ValidStatus)
            {
                UserRegistrationModel userRegModel = new UserRegistrationModel();
                LoginModel loginModel = new LoginModel();

                userRegModel.ValidStatus = true;
                loginModel.ValidStatus = true;
                loginModel.Email = decryptmail;
                userRegModel.Email = decryptmail;
                var status = _userService.updateValidStatus(userRegModel, loginModel);

                if (status)
                {
                    var x = 1;
                    ViewBag.UpdateMsg = x;
                }
                else
                {
                    ViewBag.UpdateMsg = -1;
                }

            }
            ViewBag.UpdateMsg = 0;

            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [Route("Login/{provider}")]
        public IActionResult Login(string provider, string returnUrl = null) =>
            Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var email = loginModel.Email;

                var user = _userService.findMember(email);
                var loginResponse = _userService.validateCredential(loginModel);
                if (loginResponse != null)
                {
                    if (loginResponse.ValidStatus == false)
                    {
                        ViewBag.ErrorMessage = "A verification link has been sent to your email. Please verify your account to Login.";

                        string baseUrl = string.Format("{0}://{1}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
                        var encryptmail = Base64.Base64Encode(email);
                        var activationUrl = $"{baseUrl}/Registration/ValidateUser?code.{encryptmail}.{email}";


                        string logoUrl = "https://img.freepik.com/free-vector/hand-drawn-flat-design-bookstore-logo-template_23-2149337115.jpg?w=740&t=st=1679076427~exp=1679077027~hmac=80e7c2b5fed46e9a0c585577524fe30f5b13981ed6e853c092280748b9e5216f";
                        string body = $@"<!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <title>Verify Your Bookstore Account</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    font-size: 14px;
                                    line-height: 1.5;
                                    color: #333;
                                }}
                                h1, h2, h3, h4, h5, h6 {{
                                    margin-top: 0;
                                    margin-bottom: 10px;
                                }}
                                p {{
                                    margin-top: 0;
                                    margin-bottom: 10px;
                                }}
                                a {{
                                    color: #007bff;
                                    text-decoration: none;
                                }}
                                a:hover {{
                                    color: #0056b3;
                                    text-decoration: underline;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: 0 auto;
                                    padding: 20px;
                                    border: 1px solid #ccc;
                                    border-radius: 5px;
                                }}
                                .logo {{
                                    text-align: center;
                                    margin-bottom: 20px;
                                }}
                                .logo img {{
                                    max-width: 100%;
                                    height: auto;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class=""container"">
                                <div class=""logo"">
                                    <img src=""{logoUrl}"" alt=""Bookstore"">
                                </div>
                                <h1>Verify Your Bookstore Account</h1>
                                <p>Dear Mr./Ms. {user.FirstName} {user.LastName},</p>
                                <p>You have received a request to verify your account for Bookstore. To continue verification, please click on the link below:</p>
                                <p><a href=""{activationUrl}"">Verify My Account</a></p>
                                <p>If you did not request this verification, please ignore this email.</p>
                                <p>Best regards,</p>
                                <p>The Bookstore Team</p>
                            </div>
                        </body>
                        </html>";

                        ViewBag.UpdateMsg = -1;
                        ViewBag.ErrorMessage = "You Are Not Verified Your Account.Check Your Email For Verification Link.";

                        var mailSender = new MailSender(configuration);
                        string value = mailSender.Send(user.Email, configuration["Yandex:Username"], "BookStore user account verification", body);

                        if (value == "1")
                        {

                            return View();
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed";
                            ViewBag.Message = value;
                            return View("_Unauthorised");
                        }

                    }

                }

                if (loginResponse != null && loginResponse.LoginStatus != null)
                {
                    if (loginResponse.LoginStatus == true)
                    {

                        HttpContext.Session.SetString("userId", loginModel.Email);
                        var encryptEmail = Base64.Base64Encode(loginModel.Email);

                        var userName = _userService.findMember(loginModel.Email);
                        if (userName != null)
                        {
                            HttpContext.Session.SetString("userName", userName.FirstName.ToString());
                            HttpContext.Session.SetString("Id", userName.Id.ToString());
                        }

                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.UpdateMsg = -1;
                    ViewBag.ErrorMessage = "Invalid User ID or Password";
                }
               
            }
            ViewBag.UpdateMsg = -1;
            ViewBag.ErrorMessage = "Invalid User ID or Password";
            return View();
        }



        [HttpGet]
        public IActionResult Orders(string encryptEmail)
        {
            var user = HttpContext.Session.GetString("Id");
            var o = _userService.UserOrder(user);

            List<UserOrder> userOrder = new List<UserOrder>();
            foreach (var order in o)
            {
                List<string> productTitles = order.Items.Products.Select(p => p.Title).ToList();
                var totalTitles = productTitles.Count();

                foreach (var title in productTitles)
                {
                    UserOrder userOrderItem = new UserOrder
                    {

                        OrderDate = order.OrderDate,
                        Title =title,
                        TotalAmount = order.TotalAmount
                    };
                    userOrder.Add(userOrderItem);
                }
              


               
            }

            return View(userOrder);

        }

        [HttpGet]
        public IActionResult LogOut()
        {
          
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Clear();
            
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}