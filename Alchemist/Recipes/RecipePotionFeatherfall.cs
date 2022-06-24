using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePotionFeatherfall : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Featherfall Potion";
			this.description = "Gives 30 seconds of featherfall Potion effect";
			this.debuffDuration = 30;
			this.sound = SoundID.Item85;
			this.dust = 16;
			this.buff = 8;
			this.buffDuration = 30;
			
			this.ingredients.Add(ItemType<Weapons.Nature.DaybloomFlask>());
			this.ingredients.Add(ItemType<Weapons.Fire.BlinkrootFlask>());
			this.ingredients.Add(ItemType<Weapons.Air.CloudInAVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
