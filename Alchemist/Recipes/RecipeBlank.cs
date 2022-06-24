using Terraria.ID;
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
			this.sound = SoundID.Item1;
		}
		
		
		public override void Reaction(Player player, OrchidModPlayerAlchemist modPlayer)
		{
		}
	}
}
