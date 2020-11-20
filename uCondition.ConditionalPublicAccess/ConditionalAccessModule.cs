using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.ConditionalPublicAccess.Helpers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Composing;

namespace uCondition.ConditionalPublicAccess
{
    public class ConditionalAccessModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest +=
                (sender, e) => Context_AuthenticateRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
        }

        private void Context_AuthenticateRequest(HttpContextBase context)
        {
            EnsureUmbracoContext(context);

            var umbracoContext = Current.UmbracoContext;
            var umbracoHelper = Current.UmbracoHelper;
            var contentCache = umbracoContext.Content;
            var mediaService = Current.Services.MediaService;

            IEnumerable<int> ids;

            if (context.Request.Url.AbsolutePath.StartsWith("/media", true, CultureInfo.InvariantCulture))
            {
                var media = mediaService.GetMediaByPath(context.Request.Url.AbsolutePath);
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
                IPublishedContent content = contentCache.GetByRoute(context.Request.Url.AbsolutePath, new bool?());
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
                    context.Response.Redirect($"~{contentCache.GetById(condition.FalseActionNodeId).Url()}?returnUrl={context.Request.Url.AbsolutePath}");
                }
            }

            context.Response.Redirect($"~{contentCache.GetById(protectedPage.FalseActionNodeId).Url()}?returnUrl={context.Request.Url.AbsolutePath}");
        }

        private UmbracoContext EnsureUmbracoContext(HttpContextBase context)
        {
            if (Current.UmbracoContext != null)
            {
                return Current.UmbracoContext;
            }

            // Hacky...
            var umbracoContextFactory = DependencyResolver.Current.GetService<IUmbracoContextFactory>();
            return umbracoContextFactory.EnsureUmbracoContext(context).UmbracoContext;
        }
    }
}