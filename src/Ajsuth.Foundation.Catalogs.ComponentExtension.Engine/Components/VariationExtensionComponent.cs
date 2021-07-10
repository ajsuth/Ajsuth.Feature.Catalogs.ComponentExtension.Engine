// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariationExtensionComponent.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Core;

namespace Ajsuth.Foundation.Catalogs.ComponentExtension.Engine.Components
{
    /// <summary>Defines the VariationExtension component.</summary>
    /// <seealso cref="Component" />
    public class VariationExtensionComponent : Component
    {
        public string Material { get; set; }
        public bool IsClearance { get; set; }
    }
}