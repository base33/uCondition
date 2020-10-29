using System.Linq;
using uCondition.ConditionalPublicAccess.Data;
using Umbraco.Core;
using Umbraco.Core.Composing;
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
            TreeControllerBase.TreeNodesRendering -= ContentTreeController_TreeNodesRendering;
            TreeControllerBase.MenuRendering -= ContentTreeController_MenuRendering;
        }

        private void ContentTreeController_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media" && e.NodeId == "-1")
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

            var protectMenuItemIndex = e.Menu.Items.FindIndex(x => x.Alias == "protect");
            e.Menu.Items.Insert(protectMenuItemIndex + 1, menuItem);
        }

        private void ContentTreeController_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content" && sender.TreeAlias != "media")
            {
                return;
            }

            var protectedPages = new ProtectedPageProvider().Load();

            foreach (var node in e.Nodes)
            {
                if (!int.TryParse((string)node.Id, out var nodeId))
                {
                    continue;
                }

                var itemPath = sender.TreeAlias == "content"
                               ? _contentService.GetById(nodeId)?.Path
                               : _mediaService.GetById(nodeId)?.Path;

                if (itemPath == null || string.IsNullOrWhiteSpace(itemPath))
                {
                    continue;
                }

                if (protectedPages.Pages.Any(c => c.Id == nodeId))
                {
                    node.CssClasses.Add("uConditionAccess");
                }
                else if (protectedPages.Pages.Any(
                    c => itemPath.IndexOf(c.Id.ToString()) >= 0))
                {
                    node.CssClasses.Add("uConditionAccess");
                    node.CssClasses.Add("inheritedAccess");
                }
            }
        }
    }
}