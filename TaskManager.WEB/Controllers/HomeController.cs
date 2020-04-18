using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Services;
using TaskManager.WEB.Models;

namespace TaskManager.WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDepartmentService departmentService;
        private IUserService userService;

        public HomeController(IDepartmentService departmentService, IUserService userService)
        {
            this.departmentService = departmentService;
            this.userService = userService;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Departments()
        {
            IQueryable<DepartmentDTO> departmentDTOs = departmentService.GetAll();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepartmentDTO, DepartmentViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var depVM = mapper.Map<IQueryable<DepartmentDTO>, IEnumerable<DepartmentViewModel>>(departmentDTOs).OrderBy(x => x.DName).ToList();
            return View(depVM);
        }
        public ActionResult AboutDepartment(int id)
        {
            var userDTOs = userService.GetAll().Where(x => x.DepartmentId == id).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var userVM = mapper.Map<List<UserDTO>, IEnumerable<UserViewModel>>(userDTOs);
            ViewBag.DepName = departmentService.GetAll().Where(x => x.Id == id).FirstOrDefault().DName;
            ViewBag.DepId = id;
            return View(userVM);
        }

    }
}