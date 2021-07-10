// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopulateCatalogExtensionViewActionsBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Foundation.Catalogs.ComponentExtension.Engine.Pipelines.Blocks
{
    /// <summary>Defines the synchronous executing PopulateCatalogExtensionViewActions pipeline block</summary>
    /// <seealso cref="SyncPipelineBlock{TInput, TOutput, TContext}" />
    [PipelineDisplayName(ComponentExtensionConstants.Pipelines.Blocks.PopulateCatalogExtensionViewActions)]
    public class PopulateCatalogExtensionViewActionsBlock : SyncPipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        /// <summary>Executes the pipeline block's code logic.</summary>
        /// <param name="entityView">The entity view.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="EntityView"/>.</returns>
        public override EntityView Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            // Ensure parameters are provided
            Condition.Requires(entityView, nameof(entityView)).IsNotNull();

            var viewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            var request = context.CommerceContext.GetObject<EntityViewArgument>();

            // Validate the context of the request, i.e. entity, view name, and action name
            if (!(request?.Entity is Catalog) ||
                !request.ViewName.EqualsOrdinalIgnoreCase(viewsPolicy.Master) ||
                !entityView.Name.EqualsOrdinalIgnoreCase("Extended Information") ||
                !string.IsNullOrEmpty(request.ForAction))
            {
                return entityView;
            }

            // Add action to entity view
            var actionPolicy = entityView.GetPolicy<ActionsPolicy>();
            actionPolicy.Actions.Add(
                new EntityActionView
                {
                    Name = "EditExtensionProperties",
                    DisplayName = "Edit Extension Properties",
                    Description = "Edit Extension Properties",
                    IsEnabled = true,
                    EntityView = "ExtensionProperties",
                    Icon = "edit"
                });
                

            return entityView;
        }
    }
}