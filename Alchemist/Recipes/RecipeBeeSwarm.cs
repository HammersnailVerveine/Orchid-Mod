using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeBeeSwarm : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.name = "Bee Swarm";
			this.description = "Releases a swarm of harmful bees around the player";
			this.debuffDuration = 25;
			this.soundType = 2;
			this.soundID = 97;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.PoisonVial>()); 
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}

			int itemType = ItemType<Alchemist.Weapons.Air.QueenBeeFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				if (player.strongBees && Main.rand.Next(2) == 0)
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, player.whoAmI);
				else
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI);
				}
			}
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
		}
	}
}
