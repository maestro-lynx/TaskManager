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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/adminlte").Include(//js
                        "~/Content/js/adminlte.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/jquery/jq").Include(//js
                    "~/Content/plugins/jquery/jquery.min.js"));

           bundles.Add(new ScriptBundle("~/Content/plugins/bootstrap/js/bundle").Include(//js
                    "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js"));

             bundles.Add(new ScriptBundle("~/Content/plugins/toastr/toastrjs").Include(//toastrjs
                    "~/Content/plugins/toastr/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/plugins/fontawesome/css/all").Include(//css
                "~/Content/plugins/fontawesome-free/css/all.min.css", new CssRewriteUrlTransformFixed()));


            bundles.Add(new StyleBundle("~/Content/plugins/ionicons/css").Include(//css
                "~/Content/plugins/ionicons/ionicons.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/adminlte").Include(//css
                "~/Content/css/adminlte.min.css"));
            bundles.Add(new StyleBundle("~/Fonts/sourcesans").Include(//css
                "~/Fonts/Source Sans Pro.css"));

            bundles.Add(new StyleBundle("~/Content/plugins/toastr/toastrcss").Include(//toastrcss
                "~/Content/plugins/toastr/toastr.min.css"));

        }
    }
}
