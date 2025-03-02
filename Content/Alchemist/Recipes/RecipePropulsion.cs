using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipePropulsion : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 10;
			this.sound = SoundID.Item14;
			this.dust = 15;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			player.jump = 1;
			player.velocity.Y = -15f;

			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, DustID.Obsidian);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.2f;
			}
		}
	}
}
