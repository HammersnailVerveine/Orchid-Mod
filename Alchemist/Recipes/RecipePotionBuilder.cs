using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePotionBuilder : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Builder Potion";
			this.description = "Gives 30 seconds of builder Potion effect";
			this.debuffDuration = 30;
			this.sound = SoundID.Item25;
			this.dust = 15;
			this.buff = 107;
			this.buffDuration = 30;
			
			this.ingredients.Add(ItemType<Weapons.Fire.BlinkrootFlask>());
			this.ingredients.Add(ItemType<Weapons.Air.ShiverthornFlask>());
			this.ingredients.Add(ItemType<Weapons.Nature.MoonglowFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
