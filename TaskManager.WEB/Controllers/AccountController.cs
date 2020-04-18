using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;
using TaskManager.BLL.Services;
using TaskManager.WEB.Infrastructure.Helpers;
using TaskManager.WEB.Models;

namespace TaskManager.WEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public AccountController(IUserService userService,
            IDepartmentService departmentService)
        {
            this.userService = userService;
            this.departmentService = departmentService;
        }
        #region Login
        [AllowAnonymous]
        // GET: Login
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                ClaimsIdentity claim = await userService.AuthenticateAsync(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Wrong login or password.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return Login();
        }
        #endregion

        #region Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] image = ImageConverter.ConvertImage(registerViewModel.ProfileImage);
                UserDTO userDTO = new UserDTO
                {
                    RealName = registerViewModel.RealName,
                    Surname = registerViewModel.Surname,
                    UserName = registerViewModel.UserName,
                    Password = registerViewModel.Password,
                    DepartmentId = 1,
                    Job = "Должность не указан",
                    Role = "User",
                    ProfileImage = image,
                    CreatedAt = DateTime.Now
                };
                OperationDetails operationDetails = await userService.CreateAsync(userDTO);
                if (operationDetails.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
            }
            return View(registerViewModel);
        }
        #endregion

        #region UserPhotos
        public FileContentResult ProfileImage(string id)
        {
            if (User.Identity.IsAuthenticated || !id.Equals("default"))
            {

                if (string.IsNullOrEmpty(id))
                {
                    id = User.Identity.GetUserId();
                }
                var userImage = userService.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (userImage.ProfileImage == null)
                {
                    return File(ImageConverter.DefaultProfileImage(), "image/png");
                }
                return new FileContentResult(userImage.ProfileImage, "image/png");

            }
            else
            {
                return File(ImageConverter.DefaultProfileImage(), "image/png");

            }

        }
        #endregion

        #region Details
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }

        // GET: Account/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UserDTO userDto = await userService.FindByIdAsync(id.ToString());
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDTO, UserViewModel>();
                });
                IMapper mapper = new Mapper(config);
                UserViewModel userVM = mapper.Map<UserDTO, UserViewModel>(userDto);
                ViewBag.MyDep = departmentService.GetNameById(userDto.DepartmentId);
                return View("Profile", userVM);
            }
            else
            {
                List<DepartmentDTO> departments = departmentService.GetAll().ToList();
                UserDTO userDto = await userService.FindByIdAsync(User.Identity.GetUserId());
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDTO, UserViewModel>();
                });
                IMapper mapper = new Mapper(config);
                UserViewModel userVM = mapper.Map<UserDTO, UserViewModel>(userDto);
                ViewBag.MyDep = departmentService.GetNameById(userDto.DepartmentId);
                ViewBag.Departments = new SelectList(departments, "Id", "DName", userDto.DepartmentId);
                return View("MyProfile", userVM);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(UserViewModel userViewModel, HttpPostedFileBase inputProfileImage)
        {
            if (ModelState.IsValid)
            {
                byte[] image = ImageConverter.ConvertImage(inputProfileImage);
                userViewModel.ProfileImage = image;
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserViewModel, UserDTO>();
                });
                IMapper mapper = new Mapper(config);
                UserDTO userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                OperationDetails operationDetails = await userService.UpdateAsync(userDTO);
                if (operationDetails.Succeeded)
                {
                    TempData["Message"] = operationDetails.Message;
                    TempData["Success"] = operationDetails.Succeeded;
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    TempData["Message"] = operationDetails.Message;
                    TempData["Success"] = operationDetails.Succeeded;
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            return View("MyProfile", userViewModel);
        }
        #endregion

        [HttpPost]
        public ActionResult Search(string query, int startIndex, int pageSize)
        {
            IQueryable<UserDTO> userDtos = userService.GetAllUsersWithRoles().Where(x => x.Role == "User");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            List<UserViewModel> userVM = mapper.Map<IQueryable<UserDTO>, List<UserViewModel>>(userDtos);
            userVM.Select(x =>
            {
                x.FullName = string.Format($"{x.RealName} {x.Surname}");
                x.DepartmentName = departmentService.GetNameById(x.DepartmentId);
                return x;
            }).ToList();
            if (!string.IsNullOrEmpty(query))
            {

                var results = userVM.Where(x => x.FullName.IndexOf(query, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                if (!results.Any())
                {
                    return View(model: null);
                }
                var maxPages = results.Count() / pageSize;
                var thisPage = startIndex / pageSize;
                maxPages = (results.Count() % pageSize > 0) ? maxPages++ : maxPages;
                var page = results.Skip(startIndex).Take(pageSize);
                ViewBag.MaxPages = maxPages;
                ViewBag.Query = query;
                ViewBag.ThisPage = page;
                return View(page);
            }
            else
            {
                var maxPages = userVM.Count() / pageSize;
                var thisPage = startIndex / pageSize;
                maxPages = (userVM.Count() % pageSize > 0) ? maxPages++ : maxPages;
                var page = userVM.Skip(startIndex).Take(pageSize);
                ViewBag.MaxPages = maxPages;
                ViewBag.Query = query;
                ViewBag.ThisPage = page;
                return View(page);
            }

        }

        [ChildActionOnly]
        public PartialViewResult UserMenu()
        {
            UserDTO userDto = userService.FindById(User.Identity.GetUserId());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserMenuViewModel>();
            });
            IMapper mapper = new Mapper(config);
            UserMenuViewModel userMenuVM = mapper.Map<UserDTO, UserMenuViewModel>(userDto);
            userMenuVM.DepartmentName = departmentService.GetNameById(userDto.DepartmentId);
            return PartialView("UserMenu", userMenuVM);
        }
    }
}
