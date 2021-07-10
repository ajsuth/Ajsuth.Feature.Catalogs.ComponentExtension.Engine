// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoActionEditCategoryExtensionViewBlock.cs" company="Sitecore Corporation">
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
    /// <summary>Defines the synchronous executing DoActionEditCategoryExtensionView pipeline block</summary>
    /// <seealso cref="SyncPipelineBlock{TInput, TOutput, TContext}" />
    [PipelineDisplayName(ComponentExtensionConstants.Pipelines.Blocks.DoActionEditCategoryExtensionView)]
    public class DoActionEditCategoryExtensionViewBlock : AsyncPipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        protected CommerceCommander Commander { get; set; }

        /// <summary>Initializes a new instance of the <see cref="DoActionEditCategoryExtensionViewBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public DoActionEditCategoryExtensionViewBlock(CommerceCommander commander)
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
            if (!(entity is Category) ||
                !entityView.Name.EqualsOrdinalIgnoreCase("ExtensionProperties") ||
                !entityView.Action.EqualsOrdinalIgnoreCase("EditExtensionProperties") ||
                !string.IsNullOrWhiteSpace(entityView.ItemId))
            {
                return entityView;
            }

            var category = entity as Category;
            var extensionComponent = category.GetComponent<CategoryExtensionComponent>();

            // Assign component property values from the entity view property values
            extensionComponent.ShortDescription = entityView.GetPropertyValueByName(nameof(extensionComponent.ShortDescription));

            // Persist the changes to the category
            await Commander.PersistEntity(context.CommerceContext, category).ConfigureAwait(false);

            return entityView;
        }
    }
}