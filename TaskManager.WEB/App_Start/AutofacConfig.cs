using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.WEB
{
    public static class AutofacConfig
    {
        public static void ConfigureContainer()
        {

            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //AutoMapper
            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterModule(new AutofacModule());
           
            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();

            // установка сопоставителя зависимостей
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }

}

