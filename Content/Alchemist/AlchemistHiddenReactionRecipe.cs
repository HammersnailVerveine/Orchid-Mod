using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist
{
	public abstract class AlchemistHiddenReactionRecipe
	{
		public string typeName = "blankName";
		public string name = "blank";
		public string description = "blank description";
		public List<int> ingredients = new List<int>();
		public int level = 0;
		public int debuffDuration = 0;
		public SoundStyle sound = SoundID.Item1;
		public int buff = 0;
		public int buffDuration = 0;
		public int dust = 0;

		public delegate void RecipeEffect(Player player, OrchidAlchemist modPlayer);
		public RecipeEffect recipeEffect;
		
		public static void BlankEffect(AlchemistHiddenReactionRecipe recipe, Player player, OrchidPlayer modPlayer) {}
		
		public virtual void SetDefaults() {}
		public virtual void Reaction(Player player, OrchidAlchemist modPlayer) {}

		public AlchemistHiddenReactionRecipe()
		{
			this.typeName = this.GetType().Name;
			this.name = "blank";
			this.description = "blank description";
			this.level = 0;
			this.debuffDuration = 0;
			this.sound = SoundID.Item1;
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
