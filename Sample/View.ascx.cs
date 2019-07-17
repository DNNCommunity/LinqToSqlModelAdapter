using System;
using System.Web.UI;

namespace Dnn.LinqToSqlModuleSample
{
    public partial class View : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Pages.DataTextField = "TabName";

                using (var context = new DataContext())
                {
                    Pages.DataSource = context.Tabs;
                    Pages.DataBind();
                }
            }
        }
    }
}