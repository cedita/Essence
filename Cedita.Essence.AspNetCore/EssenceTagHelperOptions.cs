// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Cedita.Essence.AspNetCore.TagHelpers;

namespace Cedita.Essence.AspNetCore
{
    public class EssenceTagHelperOptions
    {
        public EssenceTagHelperOptions()
        {
            DefaultOperatorMode = IfOperatorMode.Or;
            DefaultComparisonMode = IfComparisonMode.Match;
        }

        /// <summary>
        /// Gets or sets default Operator Mode for comparison.
        /// </summary>
        public IfOperatorMode DefaultOperatorMode { get; set; }

        /// <summary>
        /// Gets or sets default Comparison Mode for comparison.
        /// </summary>
        public IfComparisonMode DefaultComparisonMode { get; set; }

        /// <summary>
        /// Gets or sets the default class to output on a matched comparison.
        /// </summary>
        public string DefaultClassIfClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Route Based Comparison Diagnostics are enabled.
        /// </summary>
        public bool EnableRouteBasedComparisonDiagnostics { get; set; }
    }
}
