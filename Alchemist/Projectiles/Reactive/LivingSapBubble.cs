using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
	public class LivingSapBubble : AlchemistProjReactive
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sap Bubble");
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y *= 0.95f;
			Projectile.velocity.X *= 0.99f;
			Projectile.rotation += 0.02f;

			if (Main.rand.Next(20) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 85);
			int dmg = Projectile.damage;
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 102, 100, 20, false, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 102, 150, 20, false, 1.5f, 1f, 5f);
			Player targetPlayer = Main.player[Main.myPlayer];
			Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float offsetX = targetPlayer.Center.X - center.X;
			float offsetY = targetPlayer.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 300f)
			{
				if (!targetPlayer.moonLeech)
				{
					int damage = player.statLifeMax2 - player.statLife;
					if (dmg > damage)
					{
						dmg = damage;
					}
					if (dmg > 0)
					{
						player.HealEffect(dmg, true);
						player.statLife += dmg;
					}
				}
			}

			Vector2 move = Vector2.Zero;
			distance = 500f;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].friendly)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						OrchidModAlchemistNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidModAlchemistNPC>();
						modTarget.alchemistNature = 600;
					}
				}
			}
		}
	}
}