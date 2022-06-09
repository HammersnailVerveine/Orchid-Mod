using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Nirvana
{
	public class NirvanaEarth : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 0f;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 200;
			Projectile.extraUpdates = 5;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nirvana Earth Element");
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			if (Projectile.timeLeft == 200)
			{
				for (int i = 0; i < 10; i++)
				{
					int SDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
					Main.dust[SDust].velocity *= 2f;
					Main.dust[SDust].scale = (float)Main.rand.Next(70, 110) * 0.025f;
					Main.dust[SDust].noGravity = true;
				}
			}

			if (Projectile.timeLeft == 199)
				Projectile.velocity.X *= -1;

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
			Main.dust[dust].velocity /= 3f;
			Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
			Main.dust[dust].noGravity = true;

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 500f;
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
				Projectile.velocity = (20 * Projectile.velocity + move) / 10f;
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

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}