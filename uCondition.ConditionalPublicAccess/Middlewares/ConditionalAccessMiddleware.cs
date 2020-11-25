using Microsoft.Owin;
using System;
using System.Globalization;
using System.Threading.Tasks;
using uCondition.ConditionalPublicAccess.Helpers;
using uCondition.ConditionalPublicAccess.ProtectedPageProviders;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uCondition.ConditionalPublicAccess.Middlewares
{
    public class ConditionalAccessMiddleware : OwinMiddleware
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IMediaService _mediaService;
        private readonly IDomainService _domainService;
        private readonly IProtectedPageProvider _protectedPageProvider;

        public ConditionalAccessMiddleware(
            OwinMiddleware next,
            IUmbracoContextFactory umbracoContextFactory,
            IMediaService mediaService,
            IDomainService domainService,
            IProtectedPageProvider protectedPageProvider) : base(next)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _mediaService = mediaService;
            _domainService = domainService;
            _protectedPageProvider = protectedPageProvider;
        }

        public override Task Invoke(IOwinContext context)
        {
            try
            {
                ProcessConditionalAccessRequest(context);
            }
            catch (Exception ex)
            {
                // Intentionally ignored
            }

            return Next.Invoke(context);
        }

        private void ProcessConditionalAccessRequest(IOwinContext context)
        {
            var requestUrl = context.Request.Path.ToString();
            var currentDomain = _domainService.GetByName(context.Request.Uri.Host);
            var domainPrefix = currentDomain != null
                ? $"{currentDomain.RootContentId}/"
                : string.Empty;

            using (var umbracoContextReference = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var umbracoContext = umbracoContextReference.UmbracoContext;
                var contentCache = umbracoContext.Content;

                var ids = requestUrl.StartsWith("/media", true, CultureInfo.InvariantCulture)
                    ? _mediaService.GetMediaByPath(requestUrl)?.Path
                    : contentCache.GetByRoute($"{domainPrefix}{requestUrl}")?.Path;

                if (string.IsNullOrWhiteSpace(ids))
                {
                    return;
                }

                var protectedPage = _protectedPageProvider.GetLastChild(ids);
                var hasAccess = ConditionalAccess.HasAccess(protectedPage);

                if (protectedPage == null || hasAccess)
                {
                    return;
                }

                foreach (var condition in protectedPage.Conditions)
                {
                    if (ConditionalAccess.TestCondition(condition.Condition))
                    {
                        context.Response.Redirect($"{contentCache.GetById(condition.FalseActionNodeId).Url()}?returnUrl={requestUrl}");
                    }
                }

                context.Response.Redirect($"{contentCache.GetById(protectedPage.FalseActionNodeId).Url()}?returnUrl={requestUrl}");
            }
        }
    }
}