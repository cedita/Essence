// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cedita.Essence.AspNetCore.TagHelpers
{
    [HtmlTargetElement("*", Attributes = TagAttribute)]
    public class RemoveIfTagHelper : RouteBasedTagHelper
    {
        private const string TagAttribute = "remove-if-*";

        public RemoveIfTagHelper(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IOptions<EssenceTagHelperOptions> options)
            : base(httpContextAccessor, loggerFactory, options)
        {
            RemoveIfOperator = options.Value.DefaultOperatorMode;
            RemoveIfMode = options.Value.DefaultComparisonMode;

            AddRouteMatch(RouteOption.Area, () => !string.IsNullOrWhiteSpace(RemoveIfArea), () => RemoveIfArea);
            AddRouteMatch(RouteOption.Controller, () => !string.IsNullOrWhiteSpace(RemoveIfController), () => RemoveIfController);
            AddRouteMatch(RouteOption.Action, () => !string.IsNullOrWhiteSpace(RemoveIfAction), () => RemoveIfAction);
            AddRouteMatch(RouteOption.Page, () => !string.IsNullOrWhiteSpace(RemoveIfPage), () => RemoveIfPage);
        }

        public IfOperatorMode RemoveIfOperator { get; set; }

        public IfComparisonMode RemoveIfMode { get; set; }

        public string RemoveIfController { get; set; }

        public string RemoveIfAction { get; set; }

        public string RemoveIfArea { get; set; }

        public string RemoveIfPage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var match = GetComparisonResult(RemoveIfOperator, RemoveIfMode);

            if (match)
            {
                output.TagName = null;
                output.Content.SetContent(string.Empty);
            }
        }
    }
}
