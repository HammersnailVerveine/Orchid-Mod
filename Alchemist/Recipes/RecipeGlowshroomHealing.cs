using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeGlowshroomHealing : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Glowshroom Healing";
			this.description = "Heals the player for 25 health";
			this.sound = SoundID.Item25;
			this.dust = 56;
			this.ingredients.Add(ItemType<Weapons.Nature.GlowingMushroomVial>());
			this.ingredients.Add(ItemType<Weapons.Water.KingSlimeFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			if (Main.myPlayer == player.whoAmI)
				player.HealEffect(25, true);
			player.statLife += 25;
		}
	}
}
