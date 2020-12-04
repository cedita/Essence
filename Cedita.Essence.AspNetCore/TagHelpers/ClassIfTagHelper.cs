﻿// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cedita.Essence.AspNetCore.TagHelpers
{
    [HtmlTargetElement("*", Attributes = TagAttribute)]
    public class ClassIfTagHelper : RouteBasedTagHelper
    {
        private const string TagAttribute = "class-if-*";

        public ClassIfTagHelper(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IOptions<EssenceTagHelperOptions> options)
            : base(httpContextAccessor, loggerFactory, options)
        {
            ClassIfOperator = options.Value.DefaultOperatorMode;
            ClassIfMode = options.Value.DefaultComparisonMode;
            ClassIfValue = options.Value.DefaultClassIfClass;

            AddRouteMatch(RouteOption.Area, () => !string.IsNullOrWhiteSpace(ClassIfArea), () => ClassIfArea);
            AddRouteMatch(RouteOption.Controller, () => !string.IsNullOrWhiteSpace(ClassIfController), () => ClassIfController);
            AddRouteMatch(RouteOption.Action, () => !string.IsNullOrWhiteSpace(ClassIfAction), () => ClassIfAction);
            AddRouteMatch(RouteOption.Page, () => !string.IsNullOrWhiteSpace(ClassIfPage), () => ClassIfPage);
        }

        public IfOperatorMode ClassIfOperator { get; set; }

        public IfComparisonMode ClassIfMode { get; set; }

        public string ClassIfValue { get; set; }

        public string ClassIfArea { get; set; }

        public string ClassIfController { get; set; }

        public string ClassIfAction { get; set; }

        public string ClassIfPage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var match = GetComparisonResult(ClassIfOperator, ClassIfMode);

            if (match)
            {
                var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;
                output.Attributes.SetAttribute("class", $"{ClassIfValue} {classes}");
            }
        }
    }
}
