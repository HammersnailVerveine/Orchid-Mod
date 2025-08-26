using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Gambler;
using OrchidMod.Content.Shapeshifter;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Linq;
using System;

namespace OrchidMod.Common.ModSystems
{
	public class OrchidRecipes : ModSystem
	{
		public override void AddRecipeGroups()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (OrchidMod.ThoriumMod != null)
			{
				thoriumMod.TryFind<ModItem>("MeleeThorHammer", out ModItem thorsHammerMelee);
				thoriumMod.TryFind<ModItem>("RangedThorHammer", out ModItem thorsHammerRanged);
				thoriumMod.TryFind<ModItem>("MagicThorHammer", out ModItem thorsHammerMagic);
				
				RecipeGroup group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} " + thorsHammerMelee.DisplayName.ToString().Split(':').First(), thorsHammerMelee.Type, thorsHammerRanged.Type, thorsHammerMagic.Type);
				RecipeGroup.RegisterGroup("ThorsHammers", group);
			}
		}

		public override void PostAddRecipes()
		{
			bool ContentAlchemist = ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist;
			bool ContentGambler = ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler;
			bool ContentShapeshifter = ModContent.GetInstance<OrchidServerConfig>().EnableContentShapeshifter;

			if (!ContentAlchemist || !ContentGambler || !ContentShapeshifter)
			{
				Recipe recipe;

				for (int i = 0; i < Recipe.numRecipes; i++)
				{
					recipe = Main.recipe[i];
					if (!ContentAlchemist && (recipe.createItem.ModItem is OrchidModAlchemistItem || recipe.createItem.ModItem is OrchidModAlchemistMisc || recipe.createItem.ModItem is OrchidModAlchemistEquipable))
					{
						recipe.DisableRecipe();
					}

					if (!ContentGambler && (recipe.createItem.ModItem is OrchidModGamblerEquipable || recipe.createItem.ModItem is OrchidModGamblerDie || recipe.createItem.ModItem is OrchidModGamblerChipItem || recipe.createItem.ModItem is GamblerDeck))
					{
						recipe.DisableRecipe();
					}

					if (!ContentShapeshifter && recipe.createItem.ModItem is OrchidModShapeshifterItem)
					{
						recipe.DisableRecipe();
					}
				}
			}
		}
	}
}