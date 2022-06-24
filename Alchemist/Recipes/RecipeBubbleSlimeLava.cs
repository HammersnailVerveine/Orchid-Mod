using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeBubbleSlimeLava : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 3;
			this.name = "Lava Slime Bubble";
			this.description = "Creates a catalytic lava slime bubble";
			this.debuffDuration = 20;
			this.sound = SoundID.Item85;
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.HellSlimeFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayerAlchemist modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<Alchemist.Weapons.Fire.HellSlimeFlask>();
			int dmg = modPlayer.GetSecondaryDamage(itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubbleLava>();
			Vector2 vel = new Vector2(0f, -5f);
			Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, spawnProj, dmg, 0f, player.whoAmI);
		}
	}
}
