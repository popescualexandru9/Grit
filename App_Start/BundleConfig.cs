﻿using System.Web.Optimization;

namespace Grit
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui/jquery-ui.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js",
                         "~/Scripts/Chart.js",
                         "~/Scripts/chartjs-annotation/chartjs-plugin-annotation.js",
                         "~/Scripts/calendar.js",
                         "~/Scripts/underscore-min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/bootstrap.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/Chart.css",
                      "~/Content/calendar.css"
                      ));
        }
    }
}
