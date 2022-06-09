using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
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
			this.soundType = 2;
			this.soundID = 25;
			this.dust = 60;
			this.buff = BuffType<Alchemist.Buffs.AttractiteAuraSmall>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
