using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.ConditionalPublicAccess.Helpers;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uCondition.ConditionalPublicAccess.Middlewares
{
    public class ConditionalAccessMiddleware : OwinMiddleware
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IMediaService _mediaService;
        private readonly IDomainService _domainService;

        public ConditionalAccessMiddleware(
            OwinMiddleware next,
            IUmbracoContextFactory umbracoContextFactory,
            IMediaService mediaService,
            IDomainService domainService) : base(next)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _mediaService = mediaService;
            _domainService = domainService;
        }

        public override Task Invoke(IOwinContext context)
        {
            try
            {
                ProcessConditionalAccessRequest(context);
            }
            catch (Exception)
            {
                // Intentionally ignored
            }

            return Next.Invoke(context);
        }

        private void ProcessConditionalAccessRequest(IOwinContext context)
        {
            using (var umbracoContextReference = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var umbracoContext = umbracoContextReference.UmbracoContext;
                var contentCache = umbracoContext.Content;
                var requestUrl = context.Request.Path.ToString();

                IEnumerable<int> ids;

                var currentDomain = _domainService.GetByName(context.Request.Uri.Host);
                var domainPrefix = currentDomain != null
                    ? $"{currentDomain.RootContentId}/"
                    : string.Empty;

                if (requestUrl.StartsWith("/media", true, CultureInfo.InvariantCulture))
                {
                    var media = _mediaService.GetMediaByPath(requestUrl);
                    if (media == null)
                    {
                        return;
                    }

                    ids = media.Path
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => int.Parse(c));
                }
                else
                {
                    var content = contentCache.GetByRoute($"{domainPrefix}{requestUrl}");
                    if (content == null)
                    {
                        return;
                    }

                    ids = content.Path
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => int.Parse(c));
                }

                if (ids == null && !ids.Any())
                {
                    return;
                }

                var protectedPage = new ProtectedPageProvider().LoadForPath(ids);
                var hasAccess = ConditionalAccess.HasAccess(protectedPage);

                if (protectedPage == null || hasAccess)
                {
                    return;
                }

                foreach (ProtectedPageCondition condition in protectedPage.Conditions)
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