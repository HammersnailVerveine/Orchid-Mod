using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeDemonReeks : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 3;
			this.name = "Demon Reeks";
			this.description = "Demon breath projectiles will replicate";
			this.debuffDuration = 20;
			this.soundType = 2;
			this.soundID = 85;
			this.dust = 21;
			this.buff = BuffType<Alchemist.Buffs.DemonBreathFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.ShadowChestFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
