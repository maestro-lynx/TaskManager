using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using TaskManager.BLL.Services;
using TaskManager.BLL.Services.Impl;
using TaskManager.DAL.EntityFramework;
using TaskManager.DAL.Repo;
using TaskManager.DAL.Repo.Impl;

namespace TaskManager.BLL.Infrastructure
{
    public class AutofacModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DBContext>().InstancePerRequest();

            #region регистрация репозиториев

            builder.RegisterType<ProjectRepo>()
                .As<IProjectRepo>();

            builder.RegisterType<CommentRepo>()
                .As<ICommentRepo>();

            builder.RegisterType<UserRepo>()
                .As<IUserRepo>();

            builder.RegisterType<RoleRepo>()
                .As<IRoleRepo>();
            builder.RegisterType<DepartmentRepo>()
                .As<IDepartmentRepo>();

            #endregion
            #region регистрация сервисов
            builder.RegisterType<ProjectService>()
                .As<IProjectService>();

            builder.RegisterType<CommentService>()
                .As<ICommentService>();

            builder.RegisterType<RoleService>()
                .As<IRoleService>();

            builder.RegisterType<UserService>()
                .As<IUserService>();

            builder.RegisterType<DepartmentService>()
                .As<IDepartmentService>();

            #endregion
        }
    }

}

