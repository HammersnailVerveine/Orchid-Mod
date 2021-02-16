using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public abstract class AlchemistHiddenReactionRecipe
	{
		public AlchemistHiddenReaction reactionType = AlchemistHiddenReaction.NULL;
		public string reactionText = "blank";
		public List<int> reactionIngredients = new List<int>();
		
		public AlchemistHiddenReactionRecipe(AlchemistHiddenReaction rType, string rText, params int[] rItems) {
			this.reactionType = rType;
			this.reactionText = rText;
			for (int i = 0; i < rItems.Length; i++) {
				reactionIngredients.Add(rItems[i]);
			}
		}
	}
}
