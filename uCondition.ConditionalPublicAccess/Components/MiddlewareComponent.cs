using Owin;
using uCondition.ConditionalPublicAccess.Middlewares;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uCondition.ConditionalPublicAccess.Composers
{
    internal class MiddlewareComponent : IComponent
    {
        private readonly IMediaService _mediaService;
        private readonly IDomainService _domainService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public MiddlewareComponent(
            IMediaService mediaService,
            IDomainService domainService,
            IUmbracoContextFactory umbracoContextFactory)
        {
            _mediaService = mediaService;
            _domainService = domainService;
            _umbracoContextFactory = umbracoContextFactory;
        }

        public void Initialize()
        {
            UmbracoDefaultOwinStartup.MiddlewareConfigured += ConfigureMiddleware;
        }

        public void Terminate()
        {
            UmbracoDefaultOwinStartup.MiddlewareConfigured -= ConfigureMiddleware;
        }

        private void ConfigureMiddleware(object sender, OwinMiddlewareConfiguredEventArgs args)
        {
            args.AppBuilder.Use<ConditionalAccessMiddleware>(
                _umbracoContextFactory,
                _mediaService,
                _domainService);
        }
    }
}