using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{

	public class ThunderScepterOrb : OrchidModShamanProjectile
	{
		private float startX = 0;
		private float startY = 0;
		private int orbsNumber = 0;
		private bool reverseAnim;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thunder Scepter Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 34;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 12;
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

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 10 == 0)
			{
				bool done = false;

				if (player.GetModPlayer<OrchidShaman>().orbCountUnique < 10)
				{
					if (!done && Projectile.frame == 0)
					{
						Projectile.frame = 1;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 3)
					{
						Projectile.frame = 2;
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

					if (!done && Projectile.frame == 2)
					{
						if (reverseAnim)
						{
							Projectile.frame = 1;
							done = true;
						}
						else
						{
							Projectile.frame = 3;
							done = true;
						}
					}
				}

				if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 10 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 15)
				{
					if (!done && Projectile.frame == 4 || Projectile.frame < 4)
					{
						Projectile.frame = 5;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 7)
					{
						Projectile.frame = 6;
						reverseAnim = true;
						done = true;
					}

					if (!done && Projectile.frame == 5)
					{
						if (reverseAnim)
						{
							Projectile.frame = 4;
							done = true;
						}
						else
						{
							Projectile.frame = 6;
							done = true;
						}
					}

					if (!done && Projectile.frame == 6)
					{
						if (reverseAnim)
						{
							Projectile.frame = 5;
							done = true;
						}
						else
						{
							Projectile.frame = 7;
							done = true;
						}
					}
				}

				if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 15)
				{
					if (!done && Projectile.frame == 8 || Projectile.frame < 8)
					{
						Projectile.frame = 9;
						reverseAnim = false;
						done = true;
					}

					if (!done && Projectile.frame == 11)
					{
						Projectile.frame = 10;
						reverseAnim = true;
						done = true;
					}

					if (!done && Projectile.frame == 9)
					{
						if (reverseAnim)
						{
							Projectile.frame = 8;
							done = true;
						}
						else
						{
							Projectile.frame = 10;
							done = true;
						}
					}

					if (!done && Projectile.frame == 10)
					{
						if (reverseAnim)
						{
							Projectile.frame = 9;
							done = true;
						}
						else
						{
							Projectile.frame = 11;
							done = true;
						}
					}
				}
			}

			if (player.GetModPlayer<OrchidShaman>().orbCountUnique < 5
			|| player.GetModPlayer<OrchidShaman>().orbCountUnique > 20
			|| player.GetModPlayer<OrchidShaman>().shamanOrbUnique != ShamanOrbUnique.GRANDTHUNDERBIRD)
			{
				Projectile.Kill();
			}

			else orbsNumber = player.GetModPlayer<OrchidShaman>().orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(20) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 2f)
			{
				vector *= 2f / magnitude;
			}
		}

		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust;
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale = 1.75f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
