using Terraria.ID;
using Terraria;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeBlank : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 0;
			this.debuffDuration = 0;
			this.sound = SoundID.Item1;
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
