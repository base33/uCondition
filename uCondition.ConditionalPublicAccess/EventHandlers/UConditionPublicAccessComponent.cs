using System.Linq;
using uCondition.ConditionalPublicAccess.Data;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace uCondition.ConditionalPublicAccess.EventHandlers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UConditionPublicAccessComposer : ComponentComposer<UConditionPublicAccessComponent>, IUserComposer
    {
    }

    public class UConditionPublicAccessComponent : IComponent
    {
        private readonly IMediaService _mediaService;
        private readonly IContentService _contentService;

        public UConditionPublicAccessComponent(IMediaService mediaService, IContentService contentService)
        {
            _mediaService = mediaService;
            _contentService = contentService;
        }

        public void Initialize()
        {
            TreeControllerBase.TreeNodesRendering += ContentTreeController_TreeNodesRendering;
            TreeControllerBase.MenuRendering += ContentTreeController_MenuRendering;
        }

        public void Terminate()
        {
            TreeControllerBase.TreeNodesRendering += ContentTreeController_TreeNodesRendering;
            TreeControllerBase.MenuRendering += ContentTreeController_MenuRendering;
        }

        private void ContentTreeController_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media")
            {
                return;
            }

            var menuItem = new MenuItem("conditionalPublicAccess", "Conditional Access")
            {
                Icon = "code"
            };

            menuItem.AdditionalData.Add(
                "actionRoute",
                $"/{sender.TreeAlias}/{sender.TreeAlias}/uconditionaccess/{e.NodeId}");

            if (sender.TreeAlias == "content")
            {
                e.Menu.Items.Insert(12, menuItem);
            }
            else
            {
                e.Menu.Items.Insert(e.Menu.Items.Count - 1, menuItem);
            }
        }

        private void ContentTreeController_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media")
            {
                return;
            }

            var protectedPages = new ProtectedPageProvider().Load();

            if (sender.TreeAlias == "content")
            {
                foreach (var node in e.Nodes)
                {
                    var nodeId = int.Parse((string)node.Id);

                    if (nodeId <= 0)
                    {
                        continue;
                    }

                    IContent content = _contentService.GetById(nodeId);

                    if (protectedPages.Pages.Any(c => c.Id == nodeId))
                    {
                        node.CssClasses.Add("uConditionAccess");
                    }
                    else if (protectedPages.Pages.Any(
                        c => content.Path.IndexOf(c.Id.ToString()) >= 0))
                    {
                        node.CssClasses.Add("uConditionAccess");
                        node.CssClasses.Add("inheritedAccess");
                    }
                }
            }
            else
            {
                foreach (var node in e.Nodes)
                {
                    int nodeId = int.Parse((string)node.Id);

                    if (nodeId <= 0)
                    {
                        continue;
                    }

                    IMedia media = _mediaService.GetById(nodeId);

                    if (protectedPages.Pages.Any(c => c.Id == nodeId))
                    {
                        node.CssClasses.Add("uConditionAccess");
                    }
                    else if (protectedPages.Pages.Any(c => media.Path.IndexOf(c.Id.ToString()) >= 0))
                    {
                        node.CssClasses.Add("uConditionAccess");
                        node.CssClasses.Add("inheritedAccess");
                    }
                }
            }
        }
    }
}