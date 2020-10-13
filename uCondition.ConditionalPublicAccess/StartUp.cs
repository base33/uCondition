using System;
using System.Collections.Generic;
using System.Linq;
using uCondition.ConditionalPublicAccess.Data;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace uCondition.ConditionalPublicAccess
{
    public class StartUp : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            TreeControllerBase.TreeNodesRendering += ContentTreeController_TreeNodesRendering;
            TreeControllerBase.MenuRendering += ContentTreeController_MenuRendering;
        }

        private void ContentTreeController_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media")
                return;
            ProtectedPages protectedPages = new ProtectedPageProvider().Load();
            if (sender.TreeAlias == "content")
            {
                IContentService contentService = UmbracoContext.Current.Application.Services.ContentService;
                foreach (TreeNode node in (List<TreeNode>)e.Nodes)
                {
                    int nodeId = int.Parse((string)node.Id);
                    if (nodeId > 0)
                    {
                        IContent content = contentService.GetById(nodeId);
                        if (protectedPages.Pages.Any<ProtectedPage>((Func<ProtectedPage, bool>)(c => c.Id == nodeId)))
                            node.CssClasses.Add("uConditionAccess");
                        else if (protectedPages.Pages.Any<ProtectedPage>((Func<ProtectedPage, bool>)(c => content.Path.IndexOf(c.Id.ToString()) >= 0)))
                        {
                            node.CssClasses.Add("uConditionAccess");
                            node.CssClasses.Add("inheritedAccess");
                        }
                    }
                }
            }
            else
            {
                IMediaService mediaService = UmbracoContext.Current.Application.Services.MediaService;
                foreach (TreeNode node in (List<TreeNode>)e.Nodes)
                {
                    int nodeId = int.Parse((string)node.Id);
                    if (nodeId > 0)
                    {
                        IMedia media = mediaService.GetById(nodeId);
                        if (protectedPages.Pages.Any<ProtectedPage>((Func<ProtectedPage, bool>)(c => c.Id == nodeId)))
                            node.CssClasses.Add("uConditionAccess");
                        else if (protectedPages.Pages.Any<ProtectedPage>((Func<ProtectedPage, bool>)(c => media.Path.IndexOf(c.Id.ToString()) >= 0)))
                        {
                            node.CssClasses.Add("uConditionAccess");
                            node.CssClasses.Add("inheritedAccess");
                        }
                    }
                }
            }
        }

        private void ContentTreeController_MenuRendering(TreeControllerBase sender,MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media")
                return;

            MenuItem menuItem = new MenuItem("conditionalPublicAccess", "Conditional Access");
            menuItem.Icon = "code";
            menuItem.AdditionalData.Add("actionRoute", (object)(string.Format("/{0}/{1}/uconditionaccess/", (object)sender.TreeAlias, (object)sender.TreeAlias) + e.NodeId.ToString()));

            if (sender.TreeAlias == "content")
                e.Menu.Items.Insert(12, menuItem);
            else
                e.Menu.Items.Insert(e.Menu.Items.Count - 1, menuItem);
        }
    }
}
