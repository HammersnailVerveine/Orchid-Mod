using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePotionInvisibility : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Invisibility Potion";
			this.description = "Gives 30 seconds of invisibility Potion effect";
			this.debuffDuration = 30;
			this.soundType = 2;
			this.soundID = 25;
			this.dust = 15;
			this.buff = 10;
			this.buffDuration = 30;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.MoonglowFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
