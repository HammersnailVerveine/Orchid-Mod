using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeAttractiteAuraLesser : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Lesser Attractite Aura";
			this.description = "Applies attractite to nearby enemies";
			this.debuffDuration = 15;
			this.sound = SoundID.Item25;
			this.dust = 60;
			this.buff = BuffType<Buffs.AttractiteAuraSmall>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Weapons.Nature.AttractiteFlask>());
			this.ingredients.Add(ItemType<Weapons.Air.CloudInAVial>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
