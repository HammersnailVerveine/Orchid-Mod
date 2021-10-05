using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Alchemist
{
	public abstract class AlchemistHiddenReactionRecipe
	{
		public string typeName = "blankName";
		public string name = "blank";
		public string description = "blank description";
		public List<int> ingredients = new List<int>();
		public int level = 0;
		public int debuffDuration = 0;
		public int soundType = 0;
		public int soundID = 0;
		public int buff = 0;
		public int buffDuration = 0;
		public int dust = 0;

		public delegate void RecipeEffect(Player player, OrchidModPlayer modPlayer);
		public RecipeEffect recipeEffect;
		
		public static void BlankEffect(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer) {}
		
		public virtual void SetDefaults() {}
		public virtual void Reaction(Player player, OrchidModPlayer modPlayer) {}

		public AlchemistHiddenReactionRecipe()
		{
			this.typeName = this.GetType().Name;
			this.name = "blank";
			this.description = "blank description";
			this.level = 0;
			this.debuffDuration = 0;
			this.soundType = 0;
			this.soundID = 0;
			this.recipeEffect = this.Reaction;
			this.ingredients = new List<int>();
			this.buff = 0;
			this.dust = 0;
			this.buffDuration = 0;
			
			this.SetDefaults();

			string newDesc = "";
			int len = description.Length;
			int refParseLength = 32;
			int firstParse = refParseLength;

			if (len > refParseLength)
			{
				for (int i = refParseLength; i > 0; i--)
				{
					if (description[i] == ' ')
					{
						firstParse = i;
						break;
					}
				}
			}

			int secondParse = refParseLength + firstParse;

			if (len > refParseLength + firstParse)
			{
				for (int i = refParseLength + firstParse; i > firstParse + 1; i--)
				{
					if (description[i] == ' ')
					{
						secondParse = i;
						break;
					}
				}
			}

			for (int i = 0; i < description.Length; i++)
			{
				if (i == firstParse || i == secondParse)
				{
					newDesc += "\n";
				}
				else
				{
					newDesc += description[i];
				}
			}

			this.description = newDesc;
		}
	}
}
