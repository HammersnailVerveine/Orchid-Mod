using Terraria;
using Terraria.Localization;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeGlowshroomHealing : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.sound = SoundID.Item25;
			this.dust = 56;
			this.debuffDuration = 15;
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
