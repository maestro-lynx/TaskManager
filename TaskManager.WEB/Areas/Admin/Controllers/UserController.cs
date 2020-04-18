using AutoMapper;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;
using TaskManager.BLL.Services;
using TaskManager.WEB.Areas.Admin.ViewModels;
using Microsoft.AspNet.Identity;
using TaskManager.WEB.Infrastructure.Helpers;

namespace TaskManager.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;
        private readonly IRoleService roleService;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public UserController(IUserService userService, IRoleService roleService,
            IDepartmentService departmentService)
        {
            this.userService = userService;
            this.departmentService = departmentService;
            this.roleService = roleService;
        }
        // GET: Admin/User
        public ActionResult Index()
        {
            IQueryable<UserDTO> userDto =  userService.GetAllUsersWithRoles();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap <UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var userVM = mapper.Map<IQueryable<UserDTO>, IEnumerable<UserViewModel>>(userDto).OrderBy(x=>x.UserName).ToList();
            return View(userVM);
        }

        // GET: Admin/User/Details/5
        public async Task<ActionResult> Details(string id)
        {
            UserDTO userDto = await userService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            UserViewModel userVM = mapper.Map<UserDTO, UserViewModel>(userDto);
            userVM.DepartmentName = departmentService.GetNameById(userDto.DepartmentId);
            return View(userVM);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            IEnumerable<DepartmentDTO> departments = departmentService.GetAll().ToList();
            var roles = userService.GetAllRoles().Select(x => new { Role = x, Text = x }).ToList(); 
            ViewBag.Departments = departments;
            ViewBag.Role = roles;
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel userViewModel, HttpPostedFileBase inputProfileImage)
        {
            try
            {
                
                userViewModel.ProfileImage = ImageConverter.ConvertImage(inputProfileImage);
                userViewModel.CreatedAt = DateTime.Now;
                userViewModel.Id = "";
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserViewModel, UserDTO>();
                });
                IMapper mapper = new Mapper(config);
                UserDTO userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                OperationDetails operationDetails = await userService.CreateAsync(userDTO);
                if (operationDetails.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }

        // GET: Admin/User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            List<DepartmentDTO> departments = departmentService.GetAll().ToList();
            UserDTO userDto = await userService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            UserViewModel userVM = mapper.Map<UserDTO, UserViewModel>(userDto);
            List<string> roles = userService.GetAllRoles();
            ViewBag.Departments = new SelectList(departments, "Id", "DName",userVM.DepartmentId);
            ViewBag.Roles = new SelectList(roles,userVM.Role); ;
            return View(userVM);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel userVM, HttpPostedFileBase inputProfileImage)
        {
            try
            {
                userVM.ProfileImage = ImageConverter.ConvertImage(inputProfileImage);
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserViewModel, UserDTO>();
                });
                IMapper mapper = new Mapper(config);
                UserDTO userDTO = mapper.Map<UserViewModel, UserDTO>(userVM);
                OperationDetails operationDetails = await userService.UpdateAsync(userDTO);
                if (operationDetails.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            UserDTO userDto = await userService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            UserViewModel userVM = mapper.Map<UserDTO, UserViewModel>(userDto);
            return View(userVM);
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, UserViewModel userVM)
        {
            try
            {
                OperationDetails operationDetails = await userService.DeleteByIdAsync(id);
                if (operationDetails.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
