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
			Projectile.width = 14;
			Projectile.height = 40;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 9;
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
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 20 == 0)
			{
				bool done = false;

				if (modPlayer.orbCountUnique < 5)
				{
					if (!done && Projectile.frame == 0)
					{
						Projectile.frame = 1;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 2)
					{
						Projectile.frame = 1;
						reverseAnim = true;
						done = true;
					}

					if (!done && Projectile.frame == 1)
					{
						if (reverseAnim)
						{
							Projectile.frame = 0;
							done = true;
						}
						else
						{
							Projectile.frame = 2;
							done = true;
						}
					}
				}

				if (modPlayer.orbCountUnique >= 5 && modPlayer.orbCountUnique < 7)
				{
					if (!done && Projectile.frame == 3 || Projectile.frame < 3)
					{
						Projectile.frame = 4;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 5)
					{
						Projectile.frame = 4;
						reverseAnim = true;
						done = true;
					}

					if (!done && Projectile.frame == 4)
					{
						if (reverseAnim)
						{
							Projectile.frame = 3;
							done = true;
						}
						else
						{
							Projectile.frame = 5;
							done = true;
						}
					}
				}

				if (modPlayer.orbCountUnique >= 7)
				{
					if (!done && Projectile.frame == 6 || Projectile.frame < 6)
					{
						Projectile.frame = 7;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 8)
					{
						Projectile.frame = 7;
						reverseAnim = true;
						done = true;
					}

					if (!done && Projectile.frame == 7)
					{
						if (reverseAnim)
						{
							Projectile.frame = 6;
							done = true;
						}
						else
						{
							Projectile.frame = 8;
							done = true;
						}
					}
				}
			}


			if (modPlayer.orbCountUnique == 0 || modPlayer.orbCountUnique > 10 || modPlayer.shamanOrbUnique != ShamanOrbUnique.ICE)
				Projectile.Kill();

			else orbsNumber = modPlayer.orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}

			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1f;
			}

			if ((Main.mouseX + Main.screenPosition.X) < player.Center.X)
				Projectile.velocity.X = -15f;
			else
				Projectile.velocity.X = 15f;

			Projectile.velocity.Y = 6f;
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
			Player player = Main.player[Projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust;
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale = 1.75f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
