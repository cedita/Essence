// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cedita.Essence.AspNetCore.TagHelpers
{
    public abstract class RouteBasedTagHelper : ComparisonBasedTagHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<RouteBasedTagHelper> logger;
        private readonly EssenceTagHelperOptions options;

        protected RouteBasedTagHelper(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IOptions<EssenceTagHelperOptions> options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = loggerFactory.CreateLogger<RouteBasedTagHelper>();
            this.options = options.Value;
        }

        protected enum RouteOption
        {
            Area,
            Controller,
            Action,
            Page,
        }

        protected void AddRouteMatch(RouteOption option, Func<bool> qualifier, Func<string> matchTo)
        {
            AddComparison(qualifier, () =>
            {
                var optionVal = httpContextAccessor.HttpContext.GetRouteValue(option.ToString().ToLower()) as string;

                if (options.EnableRouteBasedComparisonDiagnostics)
                {
                    logger.LogDebug("RouteBasedTagHelper Comparison Value: {optionVal}", optionVal);
                }

                // Supporting multiple values, split by ;
                var matchVals = matchTo()
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Sanitise for Razor Page
                if (option == RouteOption.Page)
                {
                    for (int i = 0; i < matchVals.Length; i++)
                    {
                        if (matchVals[i][0] != '/')
                        {
                            matchVals[i] = "/" + matchVals[i];
                        }
                    }
                }

                if (!matchVals.Any(m => m.Contains("[")))
                {
                    return matchVals.Any(m => string.Equals(optionVal, m, StringComparison.InvariantCultureIgnoreCase));
                }

                // If we're checking further route values
                var enhancedMatchValues = new List<EnhancedMatch>();
                foreach (var matchVal in matchVals)
                {
                    if (!matchVal.Contains("["))
                    {
                        enhancedMatchValues.Add(new EnhancedMatch { BaseMatch = matchVal });
                        continue;
                    }

                    int matchStart = matchVal.IndexOf("[", StringComparison.InvariantCulture) + 1,
                        matchEnd = matchVal.LastIndexOf("]", StringComparison.InvariantCulture);
                    var matchSplit = matchVal.Substring(matchStart, matchEnd - matchStart).Split('=');

                    enhancedMatchValues.Add(new EnhancedMatch
                    {
                        BaseMatch = matchVal.Substring(0, matchStart - 1),
                        QueryParamName = matchSplit[0].ToLowerInvariant(),
                        QueryParamValue = matchSplit[1].ToLowerInvariant(),
                    });
                }

                return enhancedMatchValues.Any(m =>
                {
                    var baseMatch = string.Equals(optionVal, m.BaseMatch, StringComparison.InvariantCultureIgnoreCase);
                    if (!baseMatch)
                    {
                        return false;
                    }

                    if (m.QueryParamName == null)
                    {
                        return true;
                    }

                    var queryStringVal = httpContextAccessor.HttpContext.Request.Query[m.QueryParamName];
                    return string.Equals(queryStringVal, m.QueryParamValue, StringComparison.InvariantCultureIgnoreCase);
                });
            });
        }

        private class EnhancedMatch
        {
            public string BaseMatch { get; set; }

            public string QueryParamName { get; set; }

            public string QueryParamValue { get; set; }
        }
    }
}
