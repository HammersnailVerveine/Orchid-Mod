using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

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
			this.sound = SoundID.Item97;
			this.dust = 16;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.PoisonVial>()); 
		}
		
		
		public override void Reaction(Player player, OrchidModPlayerAlchemist modPlayer)
		{
			int itemType = ItemType<Alchemist.Weapons.Air.QueenBeeFlask>();
			int dmg = modPlayer.GetSecondaryDamage(itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				if (player.strongBees && Main.rand.NextBool(2))
					Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, player.whoAmI);
				else
				{
					Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, ProjectileID.Bee, dmg, 0f, player.whoAmI);
				}
			}
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, spawnProj, dmg, 0f, player.whoAmI);
			}
		}
	}
}
