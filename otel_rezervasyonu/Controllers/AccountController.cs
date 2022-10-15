using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using otel_rezervasyonu.Identity;
using otel_rezervasyonu.Models;
using System.Text;

namespace otel_rezervasyonu.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;
        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult Login()
        {
            return View(new LoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);
            }
            else
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Bu mail kayitli degildir");
                    return View(model);
                }

                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Bu mail onaylanmamistir. Lutfen mail box'inizi kontrol ediniz");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Privacy", "Home");
                }

                ModelState.AddModelError("", "Email yada parola yanliş");
                return View(model);
            }
        }
       
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("~/");
        }
        
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            else
            {
                var user = new CustomUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Tc = model.Tckimlik
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    StringBuilder mailbuilder = new StringBuilder();
                    mailbuilder.Append("<html>");
                    mailbuilder.Append("<head>");
                    mailbuilder.Append("<meta charset='" + "utf-8'" + " />");
                    mailbuilder.Append("<title> Email Onaylama</title>");
                    mailbuilder.Append("</head>");
                    mailbuilder.Append("<body>");

                    mailbuilder.Append($"<p> Merhaba {user.UserName} </p><br/>");
                    mailbuilder.Append($"Mail adresinizi onaylamak için aşagida ki linki tıklayın.<br/>");

                    mailbuilder.Append($"<a href='https://localhost:7043/ConfirmEmail/?uid={user.Id}&code={code}'>Email Onaylayın</a>");

                    mailbuilder.Append("</body>");

                    mailbuilder.Append("</html>");

                    EmailHelper helper = new EmailHelper();

                    bool isSend = helper.SendEmail(user.Email, mailbuilder.ToString());

                    if (isSend)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mail gonderilemedi");
                        return View(model);
                    } 
                }
                ModelState.AddModelError("", "büyük harf, . @ vb. Koyunuz.");
                return View(model);
            }
        }


        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string uid, string code)
        {

            ConfirmMailModel model = new ConfirmMailModel();

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(code))
            {
                var user = await userManager.FindByIdAsync(uid);

                code = code.Replace(' ', '+');

                model.Email = user.Email;

                var result = await userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var error = result.Errors.FirstOrDefault();
                    model.ErrorDescription = error.Description;
                    model.hasError = true;
                    ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }


            return View(model);
        }
    }
}
