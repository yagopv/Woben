using System;
using System.Web.Optimization;

namespace Woben.Web {
  public class BundleConfig {
	public static void RegisterBundles(BundleCollection bundles) {
	  bundles.IgnoreList.Clear();
	  AddDefaultIgnorePatterns(bundles.IgnoreList);

	  // js Vendor
	  bundles.Add(
		new ScriptBundle("~/scripts/vendor")
		  .Include("~/Scripts/jquery-{version}.js")
		  .Include("~/Scripts/bootstrap.js")
		  .Include("~/Scripts/jquery.hammer.min.js")
		  .Include("~/Scripts/Stashy.js")
		  .Include("~/Scripts/moment.js")
		  .Include("~/Scripts/marked.js")
		  .Include("~/Scripts/zen-form.js")
		);

	  // css vendor
	  bundles.Add(
		new StyleBundle("~/Content/css")
		  .Include("~/Content/font-awesome.css")
		  .Include("~/Content/Stashy.css")
		  .Include("~/Content/zen-form.css")
		  .Include("~/Content/vs.css")
		);

	  // css custom
	  bundles.Add(
		new StyleBundle("~/Content/custom")
		  .Include("~/Content/app.css")
		);
	}

	public static void AddDefaultIgnorePatterns(IgnoreList ignoreList) {
	  if(ignoreList == null) {
		throw new ArgumentNullException("ignoreList");
	  }

	  ignoreList.Ignore("*.intellisense.js");
	  ignoreList.Ignore("*-vsdoc.js");
	  ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
	}
  }
}