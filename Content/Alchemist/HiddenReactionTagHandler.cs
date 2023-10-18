using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Misc;
using OrchidMod.Content.Alchemist.UI;
using OrchidMod.Common;
using OrchidMod.Common.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace OrchidMod.Content.Alchemist
{
	public class HiddenReactionTagHandler : ITagHandler, ILoadable
	{
		void ILoadable.Load(Mod mod)
		{
			ChatManager.Register<HiddenReactionTagHandler>(new string[] { "ahr", "alchemistHiddenReaction" });
		}

		void ILoadable.Unload()
		{

		}

		// ...

		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			var recipeTypeName = AddUnnecessaryWord(text);
			var recipe = OrchidMod.AlchemistReactionRecipes.Find(r => r.typeName.Equals(recipeTypeName));

			if (recipe is null) return new TextSnippet(text);
			else return new HiddenReactionSnippet(recipe);
		}

		// ...

		public static string GenerateTag(AlchemistHiddenReactionRecipe recipe)
			=> string.Concat(new object[] { "[ahr", ":", RemoveUnnecessaryWord(recipe.typeName), "]" });

		private static string AddUnnecessaryWord(string recipeTypeName)
			=> recipeTypeName.Contains("Recipe") ? recipeTypeName : $"Recipe{recipeTypeName}";

		private static string RemoveUnnecessaryWord(string recipeTypeName)
			=> recipeTypeName.Replace("Recipe", "");

		// ...

		public class HiddenReactionSnippet : TextSnippet
		{
			private readonly AlchemistHiddenReactionRecipe recipe;

			public HiddenReactionSnippet(AlchemistHiddenReactionRecipe recipe)
			{
				this.recipe = recipe;

				Text = "«" + recipe.name + "»";
				DeleteWhole = true;
			}

			public override void OnHover()
			{
				var player = Main.LocalPlayer;

				if (!player.HasItem(ModContent.ItemType<ReactionItem>())) return;

				player.mouseInterface = true;
				Main.mouseText = true;

				if (AlchemistHiddenReactionHelper.playerKnowsRecipe(recipe, player))
				{
					Main.instance.MouseText("Click to open in the codex");
				}
				else
				{
					Main.instance.MouseText("Cannot find this entry in the codex...", ItemRarityID.Gray);
				}
			}

			public override void OnClick()
			{
				var player = Main.LocalPlayer;
				var codexType = ModContent.ItemType<ReactionItem>();

				if (player.dead) return;
				if (!player.HasItem(codexType)) return;
				if (!AlchemistHiddenReactionHelper.playerKnowsRecipe(recipe, player)) return;

				var alchemist = player.GetModPlayer<OrchidAlchemist>();

				if (!alchemist.alchemistBookUIDisplay)
				{
					UISystem.GetUIState<AlchemistBookUIState>().OpenBook(recipe);
				}

				var codexSlotIndex = -1;

				// If codex in hotbar, just open the interface
				// Otherwise, swap with the 10 slot

				for (int i = 0; i < 50; i++)
				{
					if (player.inventory[i].type.Equals(codexType))
					{
						if (i < 10)
						{
							player.selectedItem = i;
							return;
						}

						codexSlotIndex = i;
						break;
					}
				}

				(player.inventory[codexSlotIndex], player.inventory[9]) = (player.inventory[9], player.inventory[codexSlotIndex]);
				player.selectedItem = 9;
			}

			public override Color GetVisibleColor()
				=> OrchidColors.GetClassTagColor(ClassTags.Alchemist);
		}
	}
}
