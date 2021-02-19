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
		public AlchemistHiddenReaction reactionType = AlchemistHiddenReaction.NULL;
		public string reactionText = "blank";
		public string reactionDescription = "blank description";
		public List<int> reactionIngredients = new List<int>();
		
		public AlchemistHiddenReactionRecipe(AlchemistHiddenReaction rType, string rText, string rDesc = "blank description", params int[] rItems) {
			this.reactionType = rType;
			this.reactionText = rText;
			
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
