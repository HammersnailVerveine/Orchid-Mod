using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class SporeCallerProj : OrchidModShamanProjectile
	{
		bool started = false;
		int count = 0;
		int basedmg = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Spore");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.timeLeft = 1200;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.projectileTrail = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			count++;
			Projectile.friendly = started;

			if (count == 0) basedmg = Projectile.damage;
			if (count < 900) Projectile.damage = basedmg + (int)(count / 20);
			if (count > 600 && count % 100 == 0)
			{
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
			if (count > 900 && count % 50 == 0 && count % 100 != 0)
			{
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
			if (started == false)
			{
				if (count == 30) started = true;
			}
			if (started == true)
			{
				Projectile.ai[1]++;
				if (Projectile.ai[1] == 10)
				{
					Projectile.ai[1] = 0;
					Projectile.netUpdate = true;
					switch (Main.rand.Next(4))
					{
						case 0:
							Projectile.velocity.Y = 1;
							Projectile.velocity.X = 1;
							break;
						case 1:
							Projectile.velocity.Y = -1;
							Projectile.velocity.X = -1;
							break;
						case 2:
							Projectile.velocity.Y = -1;
							Projectile.velocity.X = 1;
							break;
						case 3:
							Projectile.velocity.Y = 1;
							Projectile.velocity.X = -1;
							break;
					}
				}
				for (int index1 = 0; index1 < 1; ++index1)
				{
					Projectile.velocity = Projectile.velocity * 0.75f;
				}
				if (Projectile.alpha > 70)
				{
					Projectile.alpha -= 15;
					if (Projectile.alpha < 70)
					{
						Projectile.alpha = 70;
					}
				}
				if (Projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.localAI[0] = 1f;
				}
				Vector2 move = Vector2.Zero;
				float distance = 200f;
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
					Projectile.velocity = (5 * Projectile.velocity + move) / 1f;
					AdjustMagnitude(ref Projectile.velocity);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Main.rand.Next(10) == 0) target.AddBuff((20), 5 * 60);
			if (count > 600) player.AddBuff((Mod.Find<ModBuff>("SporeEmpowerment").Type), 15 * 60);
		}
	}
}