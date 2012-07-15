using System.Web;
using System.Web.Optimization;

namespace FirePuckStore
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*", "~/Scripts/jquery.ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(BundleTheme("base"));
            bundles.Add(BundleTheme("darkness")); 
        }

        private static Bundle BundleTheme(string themeName)
        {
            return new StyleBundle(string.Format("~/Content/themes/{0}/css", themeName)).Include(
                string.Format("~/Content/themes/{0}/jquery.ui.core.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.resizable.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.selectable.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.accordion.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.autocomplete.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.button.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.dialog.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.slider.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.tabs.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.datepicker.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.progressbar.css", themeName),
                string.Format("~/Content/themes/{0}/jquery.ui.theme.css", themeName));
        }
    }
}