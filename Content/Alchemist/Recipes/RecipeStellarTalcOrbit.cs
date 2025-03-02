using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeStellarTalcOrbit : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.debuffDuration = 20;
			this.sound = SoundID.Item85;
			this.dust = 16;
			this.buff = BuffType<Alchemist.Buffs.StellarTalcBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.SunplateFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
