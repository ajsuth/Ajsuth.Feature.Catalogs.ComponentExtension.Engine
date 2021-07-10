// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoActionEditSellableItemExtensionViewBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Ajsuth.Foundation.Catalogs.ComponentExtension.Engine.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Ajsuth.Foundation.Catalogs.ComponentExtension.Engine.Pipelines.Blocks
{
    /// <summary>Defines the synchronous executing DoActionEditSellableItemExtensionView pipeline block</summary>
    /// <seealso cref="SyncPipelineBlock{TInput, TOutput, TContext}" />
    [PipelineDisplayName(ComponentExtensionConstants.Pipelines.Blocks.DoActionEditSellableItemExtensionView)]
    public class DoActionEditSellableItemExtensionViewBlock : AsyncPipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        protected CommerceCommander Commander { get; set; }

        /// <summary>Initializes a new instance of the <see cref="DoActionEditSellableItemExtensionViewBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public DoActionEditSellableItemExtensionViewBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        /// <summary>Executes the pipeline block's code logic.</summary>
        /// <param name="entityView">The entity view.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="EntityView"/>.</returns>
        public override async Task<EntityView> RunAsync(EntityView entityView, CommercePipelineExecutionContext context)
        {
            // Ensure parameters are provided
            Condition.Requires(entityView, nameof(entityView)).IsNotNull();

            var viewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            // Validate the context of the request, i.e. entity, view name, and action name
            var entity = context.CommerceContext.GetObject<CommerceEntity>(p => p.Id.EqualsOrdinalIgnoreCase(entityView.EntityId));
            if (!(entity is SellableItem) ||
                !entityView.Name.EqualsOrdinalIgnoreCase("ExtensionProperties") ||
                !entityView.Action.EqualsOrdinalIgnoreCase("EditExtensionProperties") ||
                !string.IsNullOrWhiteSpace(entityView.ItemId))
            {
                return entityView;
            }

            var sellableItem = entity as SellableItem;
            var extensionComponent = sellableItem.GetComponent<SellableItemExtensionComponent>();

            // Assign component property values from the entity view property values
            extensionComponent.CountryOfOrigin = entityView.GetPropertyValueByName(nameof(extensionComponent.CountryOfOrigin));
            extensionComponent.EnergyRating = int.Parse(entityView.GetPropertyValueByName(nameof(extensionComponent.EnergyRating)));

            // Persist the changes to the sellable item
            await Commander.PersistEntity(context.CommerceContext, sellableItem).ConfigureAwait(false);

            return entityView;
        }
    }
}