using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Gambler;
using OrchidMod.Content.Shapeshifter;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common.ModSystems
{
	public class OrchidRecipes : ModSystem
	{
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

					if (!ContentGambler && (recipe.createItem.ModItem is OrchidModGamblerEquipable || recipe.createItem.ModItem is OrchidModGamblerDie || recipe.createItem.ModItem is OrchidModGamblerChipItem))
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