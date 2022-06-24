using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePotionNightOwl : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Night Owl Potion";
			this.description = "Gives 30 seconds of night owl Potion effect";
			this.debuffDuration = 30;
			this.sound = SoundID.Item25;
			this.dust = 15;
			this.buff = 12;
			this.buffDuration = 30;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.DaybloomFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayerAlchemist modPlayer)
		{
		}
	}
}
