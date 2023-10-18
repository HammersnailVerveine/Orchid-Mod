using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class TerraScepterOrbHoming2 : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terra Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Main.projFrames[Projectile.type] = 10;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Projectile.timeLeft = 350;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft == 349 && player.GetModPlayer<OrchidShaman>().shamanOrbUnique == ShamanOrbUnique.TERRA)
			{
				player.GetModPlayer<OrchidShaman>().orbCountUnique = 0;
			}

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame == 10)
				Projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 157);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance;
			if (Projectile.timeLeft < 300)
				distance = 1000f;
			else distance = 10f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target)
			{
				AdjustMagnitude(ref move);
				Projectile.velocity = (10 * Projectile.velocity + move) / 7f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 157);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 5f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}
