using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
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
			this.soundType = 2;
			this.soundID = 85;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.SlimeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.EmberVial>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.SlimeFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}
	}
}
