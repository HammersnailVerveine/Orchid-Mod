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
	public class AlchemistHiddenReactionRecipe
	{
		public AlchemistHiddenReactionType reactionType = AlchemistHiddenReactionType.NULL;
		public string reactionText = "blank";
		public string reactionDescription = "blank description";
		public List<int> reactionIngredients = new List<int>();
		public int reactionLevel = 0;
		public int debuffDuration = 0;
		public int soundType = 0;
		public int soundID = 0;
		
		public delegate void RecipeEffect(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer);
		public RecipeEffect recipeEffect;
		
		public AlchemistHiddenReactionRecipe(AlchemistHiddenReactionType rType, int rLevel, string rText, string rDesc, int rDuration, int rSoundType, int rSoundID, RecipeEffect rRecipeEffect, params int[] rItems) {
			this.reactionType = rType;
			this.reactionText = rText;
			this.reactionLevel = rLevel;
			this.debuffDuration = rDuration;
			this.soundType = rSoundType;
			this.soundID = rSoundID;
			this.recipeEffect = rRecipeEffect;
			
			string newDesc = "";
			int len = rDesc.Length;
			int refParseLength = 32;
			int firstParse = refParseLength;
			
			if (len > refParseLength) {
				for (int i = refParseLength; i > 0; i--)
				{
					if (rDesc[i] == ' ') {
						firstParse = i;
						break;
					}
				}
			}
			
			int secondParse = refParseLength + firstParse;
			
			if (len > refParseLength + firstParse) {
				for (int i = refParseLength + firstParse; i > firstParse + 1; i--)
				{
					if (rDesc[i] == ' ') {
						secondParse = i;
						break;
					}
				}
			}
			
			for (int i = 0; i < rDesc.Length; i++)
			{
				if (i == firstParse ||i == secondParse) {
					newDesc += "\n";
				} else {
					newDesc += rDesc[i];
				}
			}
			
			this.reactionDescription = newDesc;
			for (int i = 0; i < rItems.Length; i++) {
				reactionIngredients.Add(rItems[i]);
			}
		}
	}
}
