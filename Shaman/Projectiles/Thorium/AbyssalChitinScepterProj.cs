using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class AbyssalChitinScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Bubble");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.alpha = 196;
			Projectile.timeLeft = 1800;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		// public override Color? GetAlpha(Color lightColor)
		// {
		// return Color.White;
		// }

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			Projectile.rotation += 0.1f;

			if (Projectile.timeLeft % (15 - OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) * 2) == 0)
			{
				if (Projectile.damage < (150) * modPlayer.shamanDamage)
				{
					spawnDustCircle(111, 10);
					Projectile.damage++;
				}
				else
				{
					spawnDustCircle(111, 15);
				}
			}

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 101);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			if (Projectile.timeLeft < 1760)
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

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 0.75f;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].noGravity = true;
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
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 101);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}