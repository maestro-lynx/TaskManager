using System.Web;
using System.Web.Optimization;

namespace TaskManager.WEB
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                    "~/Content/plugins/jquery/jquery.min.js",
                    "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",
                    "~/Content/js/adminlte.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmaskjs").Include(
                    "~/Content/plugins/inputmask/min/jquery.inputmask.bundle.min.js"));

             bundles.Add(new ScriptBundle("~/bundles/toastrjs").Include(
                    "~/Content/plugins/toastr/toastr.min.js",
                    "~/Scripts/MyToastr.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/plugins/fontawesome-free/css/all.min.css",
                "~/Content/plugins/ionicons/ionicons.min.css",
                "~/Content/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                "~/Content/css/adminlte.min.css",
                "~/Fonts/Source Sans Pro.css"));

            bundles.Add(new StyleBundle("~/Content/toastrcss").Include(
                "~/Content/plugins/toastr/toastr.min.css"));

        }
    }
}
