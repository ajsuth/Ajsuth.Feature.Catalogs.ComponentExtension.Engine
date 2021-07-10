// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SellableItemExtensionComponent.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Core;

namespace Ajsuth.Foundation.Catalogs.ComponentExtension.Engine.Components
{
    /// <summary>Defines the SellableItemExtension component.</summary>
    /// <seealso cref="Component" />
    public class SellableItemExtensionComponent : Component
    {
        public string CountryOfOrigin { get; set; }
        public int EnergyRating { get; set; }
    }
}