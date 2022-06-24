using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeBurningSamples : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Burning Samples";
			this.description = "Using slimy samples with a fire element will release damaging embers";
			this.debuffDuration = 15;
			this.sound = SoundID.Item85;
			this.dust = 29;
			this.buff = BuffType<Alchemist.Buffs.SlimeFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.SlimeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.EmberVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
