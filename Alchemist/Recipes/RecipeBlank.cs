using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeBlank : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.name = "blank";
			this.description = "blank description";
			this.level = 0;
			this.debuffDuration = 0;
			this.soundType = 0;
			this.soundID = 0;
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
