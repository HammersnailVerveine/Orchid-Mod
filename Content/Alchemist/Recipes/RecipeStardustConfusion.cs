using Terraria;
using Terraria.Localization;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeStardustConfusion : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 10;
			this.sound = SoundID.Item25;
			this.dust = 60;
			this.buff = BuffType<Buffs.StardustSamplesBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Weapons.Nature.SunflowerFlask>());
			this.ingredients.Add(ItemType<Weapons.Fire.StardustSamples>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
