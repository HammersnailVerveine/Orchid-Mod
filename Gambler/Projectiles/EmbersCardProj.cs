using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Projectiles
{
	public class EmbersCardProj : OrchidModGamblerProjectile
	{
		bool started = false;
		int count = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Embers");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.timeLeft = 180;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			this.count++;
			Projectile.rotation += 0.1f;

			if (Projectile.wet)
			{
				Projectile.Kill();
			}

			if (Projectile.timeLeft > 120)
			{
				Projectile.velocity.Y += 0.01f;
				Projectile.velocity.X *= 0.95f;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

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

				// for (int index1 = 0; index1 < 1; ++index1)
				// {	
				// projectile.velocity = projectile.velocity * 0.75f;		
				// }

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
				float distance = 150f;
				bool target = false;
				bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && ((!dummy && Main.npc[k].type != NPCID.TargetDummy) || (dummy && Main.npc[k].type == NPCID.TargetDummy)) && Projectile.timeLeft < 240)
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
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].velocity *= 3f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerGambler modPlayer)
		{
			target.AddBuff(BuffID.OnFire, modPlayer.gamblerElementalLens ? 60 * 5 : 60 * 1);
		}
	}
}