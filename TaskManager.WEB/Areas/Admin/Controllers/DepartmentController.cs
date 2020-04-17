using AutoMapper;
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

namespace TaskManager.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private IDepartmentService departmentService;
        private IUserService userService;

        public DepartmentController(IDepartmentService departmentService, IUserService userService)
        {
            this.departmentService = departmentService;
            this.userService = userService;

        }
        // GET: Admin/Department
        public ActionResult Index()
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

        // GET: Admin/Department/Details/5
        public ActionResult Details(int id)
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

        // GET: Admin/Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentViewModel DepVM)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DepartmentViewModel, DepartmentDTO>();
                });
                IMapper mapper = new Mapper(config);
                DepartmentDTO DepDTO = mapper.Map<DepartmentViewModel, DepartmentDTO>(DepVM);
                OperationDetails operationDetails = await departmentService.CreateAsync(DepDTO);
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
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Department/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            DepartmentDTO departmentDTO = await departmentService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepartmentDTO, DepartmentViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var depVM = mapper.Map<DepartmentDTO, DepartmentViewModel>(departmentDTO);
            return View(depVM);
        }

        // POST: Admin/Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, DepartmentViewModel DepVM)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DepartmentViewModel, DepartmentDTO>();
                });
                IMapper mapper = new Mapper(config);
                DepartmentDTO DepDTO = mapper.Map<DepartmentViewModel, DepartmentDTO>(DepVM);
                OperationDetails operationDetails = await departmentService.UpdateByIdAsync(DepDTO);
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
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Department/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            DepartmentDTO departmentDTO = await departmentService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepartmentDTO, DepartmentViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var depVM = mapper.Map<DepartmentDTO, DepartmentViewModel>(departmentDTO);
            return View(depVM);
        }

        // POST: Admin/Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, DepartmentViewModel depVM)
        {
            try
            {
                OperationDetails operationDetails = await departmentService.DeleteByIdAsync(id);
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
                return RedirectToAction("Index");
            }
        }
    }
}
