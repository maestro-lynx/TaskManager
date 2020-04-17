using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Services;
using Microsoft.AspNet.Identity;
using AutoMapper;
using TaskManager.WEB.Models;
using TaskManager.BLL.Infrastructure;
using System.Threading.Tasks;
using TaskManager.WEB.Infrastructure.Utils;

namespace TaskManager.WEB.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly IDepartmentService departmentService;
        private readonly ICommentService commentService;
        public ProjectsController(IUserService userService, ICommentService commentService,
            IProjectService projectService, IDepartmentService departmentService)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.departmentService = departmentService;
            this.commentService = commentService;
        }

        // GET: Projects/Delivered
        public ActionResult Delivered()
        {
            IQueryable<ProjectDTO> projectsDTO = projectService.GetUserDeliveredById(User.Identity.GetUserId());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectDTO, ProjectViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var projectVM = mapper.Map<IQueryable<ProjectDTO>, IEnumerable<ProjectViewModel>>(projectsDTO);
            projectVM.OrderBy(o => o.CreatedAt)
                .Select(s => s.ToUserName = userService.GetUserFullNameById(s.ToUserId));
            return View(projectVM);
        }
        // GET: Projects/Received
        public ActionResult Received()
        {
            IQueryable<ProjectDTO> projectsDTO = projectService.GetUserReceivedById(User.Identity.GetUserId());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectDTO, ProjectViewModel>();
            });
            IMapper mapper = new Mapper(config);
            var projectVM = mapper.Map<IQueryable<ProjectDTO>, IEnumerable<ProjectViewModel>>(projectsDTO);
            projectVM.OrderBy(o => o.CreatedAt)
                .Select(s => s.FromUserName = userService.GetUserFullNameById(s.FromUserId));
            return View(projectVM);
        }
        // GET: Projects/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ProjectDTO projectDTO = await projectService.FindByIdAsync(id);
            if (projectDTO.ToUserId == User.Identity.GetUserId() || projectDTO.FromUserId == User.Identity.GetUserId())
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProjectDTO, ProjectViewModel>();
                    cfg.CreateMap<CommentDTO, CommentViewModel>();
                });
                IMapper mapper = new Mapper(config);
                var projectVM = mapper.Map<ProjectDTO, ProjectViewModel>(projectDTO);
                projectVM.ToUserName = userService.GetUserFullNameById(projectVM.ToUserId);
                projectVM.FromUserName = userService.GetUserFullNameById(projectVM.FromUserId);
                var commentDTOs = commentService.GetByProjectId(projectVM.Id);
                var commentVMs = mapper.Map<IQueryable<CommentDTO>, List<CommentViewModel>>(commentDTOs)
                    .Select(x => { x.UserName = userService.GetUserFullNameById(x.UserId); return x; }).ToList();
                ViewData["Comments"] = commentVMs;
                ViewBag.MyId = User.Identity.GetUserId();
                return View(projectVM);
            }
            return View(viewName: "AccessDenied");

        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
            IMapper mapper = new Mapper(config);
            IQueryable<UserDTO> userDTOs = userService.GetAll()
                .Where(x => x.DepartmentId == userService.FindById(User.Identity.GetUserId()).DepartmentId && x.Id != User.Identity.GetUserId());
            var userVM = mapper.Map<IQueryable<UserDTO>, List<UserViewModel>>(userDTOs);
            userVM.Select(x => { x.FullName = userService.GetUserFullNameById(x.Id); return x; }).ToList();
            List<SelectListItem> statusItems = new List<SelectListItem>();
            statusItems.Add(new SelectListItem { Text = "Выполняется", Value = "Выполняется" });
            statusItems.Add(new SelectListItem { Text = "Отменен", Value = "Отменен" });
            statusItems.Add(new SelectListItem { Text = "Завершен", Value = "Завершен" });
            ViewBag.StatusItems = new SelectList(statusItems, "Value", "Text");
            ViewBag.Users = new SelectList(userVM, "Id", "FullName");
            ViewBag.FromUserId = User.Identity.GetUserId();
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectViewModel projectVM, bool sendEmail)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProjectViewModel, ProjectDTO>();
                });
                IMapper mapper = new Mapper(config);
                ProjectDTO projectDTO = mapper.Map<ProjectViewModel, ProjectDTO>(projectVM);
                projectDTO.FromUserId = User.Identity.GetUserId();
                projectDTO.CreatedAt = DateTime.Now;
                projectDTO.Progress = 0;
                projectDTO.Status = "Выполняется";
                OperationDetails operationDetails = await projectService.CreateAsync(projectDTO);
                if (operationDetails.Succeeded)
                {
                    if (sendEmail)
                    {
                        var receiver = await userService.FindByIdAsync(projectDTO.ToUserId);
                        var sender = await userService.FindByIdAsync(projectDTO.FromUserId);
                        var receiverFullName = string.Format(receiver.RealName + " " + receiver.Surname);
                        var senderFullName = string.Format(sender.RealName + " " + sender.Surname);
                        var subject = string.Format("Вам пришло задание от " + senderFullName + ".");
                        var body = string.Format("<html><body>Здравствуйте уважаемый(ая) " + receiverFullName + @" !
                            <br>Вам пришло новое задание от " + senderFullName + @".
                            <br>" + projectDTO.Title + @".
                            <br>    " + projectDTO.Description + @".                                                                                              
                            <br>Конечный срок:       <b>" + projectDTO.Deadline.ToString("dd-MM-yyyy") + @"</b>
                            <br>
                            <br>С уважением команда TaskManager!</body></html>");
                        if (!EmailService.SendEmail(receiver.Email, receiverFullName, subject, body))
                        {
                            TempData["Message"] = string.Format(operationDetails.Message + ". Произошла ошибка при отправке сообщения");
                            TempData["Success"] = operationDetails.Succeeded;
                            return RedirectToAction("Delivered", "Projects");
                        }
                    }

                }
                TempData["Message"] = string.Format(operationDetails.Message + ". Сообщение отправлено успешно");
                TempData["Success"] = operationDetails.Succeeded;
                return RedirectToAction("Delivered", "Projects");
            }
            return RedirectToAction("Delivered", "Projects");
        }

        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ProjectDTO projectDTO = await projectService.FindByIdAsync(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserViewModel>();
                cfg.CreateMap<ProjectDTO, ProjectViewModel>();
            });
            IMapper mapper = new Mapper(config);
            ProjectViewModel projectVM = mapper.Map<ProjectDTO, ProjectViewModel>(projectDTO);
            IQueryable<UserDTO> userDTOs = userService.GetAll()
                .Where(x => x.DepartmentId == userService.FindById(User.Identity.GetUserId()).DepartmentId && x.Id != User.Identity.GetUserId());
            var userVM = mapper.Map<IQueryable<UserDTO>, List<UserViewModel>>(userDTOs);
            userVM.Select(x => { x.FullName = userService.GetUserFullNameById(x.Id); return x; }).ToList();
            List<SelectListItem> statusItems = new List<SelectListItem>();
            statusItems.Add(new SelectListItem { Text = "Выполняется", Value = "Выполняется" });
            statusItems.Add(new SelectListItem { Text = "Отменен", Value = "Отменен" });
            statusItems.Add(new SelectListItem { Text = "Завершен", Value = "Завершен" });
            ViewBag.StatusItems = new SelectList(statusItems, "Value", "Text", projectVM.Status);
            ViewBag.Users = new SelectList(userVM, "Id", "FullName", projectVM.ToUserId);
            ViewBag.FromUserId = User.Identity.GetUserId();
            return View(projectVM);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ProjectViewModel projectVM, bool sendEmail)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProjectViewModel, ProjectDTO>();
                });
                IMapper mapper = new Mapper(config);
                ProjectDTO projectDTO = mapper.Map<ProjectViewModel, ProjectDTO>(projectVM);
                projectDTO.FromUserId = User.Identity.GetUserId();
                OperationDetails operationDetails = await projectService.UpdateByIdAsync(projectDTO);
                if (operationDetails.Succeeded)
                {
                    if (sendEmail)
                    {
                        var receiver = await userService.FindByIdAsync(projectDTO.ToUserId);
                        var sender = await userService.FindByIdAsync(projectDTO.FromUserId);
                        var receiverFullName = string.Format(receiver.RealName + " " + receiver.Surname);
                        var senderFullName = string.Format(sender.RealName + " " + sender.Surname);
                        var subject = string.Format("" + senderFullName + "изменил задание.");
                        var body = string.Format("<html><body>Здравствуйте уважаемый(ая) " + receiverFullName + @" !
                            <br>Изменения в задание от " + senderFullName + @".
                            <br>" + projectDTO.Title + @".
                            <br>    " + projectDTO.Description + @".
                            <br>Статус:       <b>" + projectDTO.Status + @"</b>
                            <br>Конечный срок:       <b>" + projectDTO.Deadline.ToString("dd-MM-yyyy") + @"</b>
                            <br>
                            <br>С уважением команда TaskManager!</body></html>");
                        if (!EmailService.SendEmail(receiver.Email, receiverFullName, subject, body))
                        {
                            TempData["Message"] = string.Format(operationDetails.Message + ". Произошла ошибка при отправке сообщения");
                            TempData["Success"] = operationDetails.Succeeded;
                            return RedirectToAction("Delivered", "Projects");
                        }
                        else
                        {
                            TempData["Message"] = string.Format(operationDetails.Message + ". Сообщение отправлено успешно");
                            TempData["Success"] = operationDetails.Succeeded;
                            return RedirectToAction("Delivered", "Projects");
                        }
                    }
                    TempData["Message"] = string.Format(operationDetails.Message);
                    TempData["Success"] = operationDetails.Succeeded;
                    return RedirectToAction("Delivered", "Projects");
                }


            }
            return RedirectToAction("Delivered", "Projects");
        }

        // POST: Projects/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            OperationDetails operationDetails = await projectService.DeleteByIdAsync(id);
            TempData["Message"] = operationDetails.Message;
            TempData["Success"] = operationDetails.Succeeded;
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeProgress(ProjectViewModel projectVM, int sliderProgress, bool sendEmail)
        {
            if (ModelState.IsValid)
            {
                projectVM.Progress = sliderProgress;
                if (sliderProgress == 100)
                {
                    projectVM.Status = "Выполнен";
                }
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProjectViewModel, ProjectDTO>();
                });
                IMapper mapper = new Mapper(config);
                ProjectDTO projectDTO = mapper.Map<ProjectViewModel, ProjectDTO>(projectVM);
                OperationDetails operationDetails = await projectService.UpdateByIdAsync(projectDTO);
                if (operationDetails.Succeeded)
                {
                    if (sendEmail)
                    {
                        var receiver = await userService.FindByIdAsync(projectDTO.ToUserId);
                        var sender = await userService.FindByIdAsync(projectDTO.FromUserId);
                        var receiverFullName = string.Format(receiver.RealName + " " + receiver.Surname);
                        var senderFullName = string.Format(sender.RealName + " " + sender.Surname);
                        string subject;
                        string body;
                        if (sliderProgress >= 100)
                        {
                            subject = string.Format("" + senderFullName + " выполнил ваше задание.");
                            body = string.Format("<html><body>Здравствуйте уважаемый(ая) " + receiverFullName + @" !
                            <br>Выполнено задание" + @".
                            <br>" + projectDTO.Title + @".
                            <br>    " + projectDTO.Description + @".   
                            <br>Исполнитель:       <b>" + senderFullName + @"</b>
                            <br>Конечный срок:       <b>" + projectDTO.Deadline.ToString("dd-MM-yyyy") + @"</b>
                            <br>
                            <br>С уважением команда TaskManager!</body></html>");
                        }
                        else
                        {
                            subject = string.Format("" + senderFullName + " изменил прогресс вашего задания.");
                            body = string.Format("<html><body>Здравствуйте уважаемый(ая) " + receiverFullName + @" !
                            <br>Прогресс задание изменился на " + projectDTO.Progress + @".
                            <br>" + projectDTO.Title + @".
                            <br>    " + projectDTO.Description + @".   
                            <br>Исполнитель:       <b>" + senderFullName + @"</b>
                            <br>Конечный срок:       <b>" + projectDTO.Deadline.ToString("dd-MM-yyyy") + @"</b>
                            <br>
                            <br>С уважением команда TaskManager!</body></html>");
                        }
                        if (!EmailService.SendEmail(receiver.Email, receiverFullName, subject, body))
                        {
                            TempData["Message"] = string.Format(operationDetails.Message + ". Произошла ошибка при отправке сообщения");
                            TempData["Success"] = operationDetails.Succeeded;
                            return Redirect(Request.UrlReferrer.PathAndQuery);
                        }
                        else
                        {
                            TempData["Message"] = string.Format(operationDetails.Message + ". Сообщение отправлено успешно");
                            TempData["Success"] = operationDetails.Succeeded;
                            return Redirect(Request.UrlReferrer.PathAndQuery);
                        }
                    }
                    TempData["Message"] = string.Format(operationDetails.Message);
                    TempData["Success"] = operationDetails.Succeeded;
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }


            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentViewModel commentVM)
        {
            commentVM.UserId = User.Identity.GetUserId();
            commentVM.CreatedAt = DateTime.Now;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommentViewModel, CommentDTO>();
            });
            IMapper mapper = new Mapper(config);
            CommentDTO commentDTO = mapper.Map<CommentViewModel, CommentDTO>(commentVM);
            OperationDetails operationDetails = await commentService.CreateAsync(commentDTO);
            TempData["Message"] = operationDetails.Message;
            TempData["Success"] = operationDetails.Succeeded;
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteComment(int id)
        {
            OperationDetails operationDetails = await commentService.DeleteByIdAsync(id);
            TempData["Message"] = operationDetails.Message;
            TempData["Success"] = operationDetails.Succeeded;
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}
