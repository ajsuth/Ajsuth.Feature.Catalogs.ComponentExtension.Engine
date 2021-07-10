// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace Ajsuth.Foundation.Catalogs.ComponentExtension.Engine
{
    /// <summary>The configure sitecore class.</summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>The configure services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Rules(config => config.Registry(registry => registry.RegisterAssembly(assembly)));

            services.Sitecore().Pipelines(pipelines => pipelines

                .ConfigurePipeline<IGetEntityViewPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.GetCatalogExtensionViewBlock>().After<PopulateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.GetCategoryExtensionViewBlock>().After<PopulateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.GetSellableItemExtensionViewBlock>().After<PopulateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.GetVariationExtensionViewBlock>().After<PopulateEntityVersionBlock>()
                )

                .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.PopulateCatalogExtensionViewActionsBlock>().After<InitializeEntityViewActionsBlock>()
                    .Add<Pipelines.Blocks.PopulateCategoryExtensionViewActionsBlock>().After<InitializeEntityViewActionsBlock>()
                    .Add<Pipelines.Blocks.PopulateSellableItemExtensionViewActionsBlock>().After<InitializeEntityViewActionsBlock>()
                    .Add<Pipelines.Blocks.PopulateVariationExtensionViewActionsBlock>().After<InitializeEntityViewActionsBlock>()
                )

                .ConfigurePipeline<IDoActionPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.DoActionEditCatalogExtensionViewBlock>().After<ValidateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.DoActionEditCategoryExtensionViewBlock>().After<ValidateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.DoActionEditSellableItemExtensionViewBlock>().After<ValidateEntityVersionBlock>()
                    .Add<Pipelines.Blocks.DoActionEditVariationExtensionViewBlock>().After<ValidateEntityVersionBlock>()
                )

            );
        }
    }
}
