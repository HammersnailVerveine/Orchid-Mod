using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IceSpearOrb : OrchidModShamanProjectile
	{
		private float startX = 0;
		private float startY = 0;
		private int orbsNumber = 0;
		private bool reverseAnim;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear Orb");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 40;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 9;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 20 == 0)
			{
				bool done = false;

				if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 5)
				{
					if (!done && projectile.frame == 0)
					{
						projectile.frame = 1;
						reverseAnim = false;
						done = true;
					}

					if (!done && projectile.frame == 2)
					{
						projectile.frame = 1;
						reverseAnim = true;
						done = true;
					}

					if (!done && projectile.frame == 1)
					{
						if (reverseAnim)
						{
							projectile.frame = 0;
							done = true;
						}
						else
						{
							projectile.frame = 2;
							done = true;
						}
					}
				}

				if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 5 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 7)
				{
					if (!done && projectile.frame == 3 || projectile.frame < 3)
					{
						projectile.frame = 4;
						reverseAnim = false;
						done = true;
					}

					if (!done && projectile.frame == 5)
					{
						projectile.frame = 4;
						reverseAnim = true;
						done = true;
					}

					if (!done && projectile.frame == 4)
					{
						if (reverseAnim)
						{
							projectile.frame = 3;
							done = true;
						}
						else
						{
							projectile.frame = 5;
							done = true;
						}
					}
				}

				if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 7)
				{
					if (!done && projectile.frame == 6 || projectile.frame < 6)
					{
						projectile.frame = 7;
						reverseAnim = false;
						done = true;
					}

					if (!done && projectile.frame == 8)
					{
						projectile.frame = 7;
						reverseAnim = true;
						done = true;
					}

					if (!done && projectile.frame == 7)
					{
						if (reverseAnim)
						{
							projectile.frame = 6;
							done = true;
						}
						else
						{
							projectile.frame = 8;
							done = true;
						}
					}
				}
			}


			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 10 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.ICE)
				projectile.Kill();

			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountUnique;

			if (projectile.timeLeft == 12960000)
			{
				startX = projectile.position.X - player.position.X;
				startY = projectile.position.Y - player.position.Y;
			}

			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1f;
			}

			if ((Main.mouseX + Main.screenPosition.X) < player.Center.X)
				projectile.velocity.X = -15f;
			else
				projectile.velocity.X = 15f;

			projectile.velocity.Y = 6f;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 2f)
			{
				vector *= 2f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust;
				dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale = 1.75f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
