using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{

	public class MonowaiOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monowai Orb");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 24;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}

			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 65)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 70)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 75)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 80)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 85)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 90)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 95)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 100)
				Projectile.frame = 8;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 105)
				Projectile.frame = 9;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 110)
				Projectile.frame = 10;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 115)
				Projectile.frame = 11;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 0)
				Projectile.frame = 12;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 5)
				Projectile.frame = 13;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 10)
				Projectile.frame = 14;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 15)
				Projectile.frame = 15;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 20)
				Projectile.frame = 16;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 25)
				Projectile.frame = 17;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 30)
				Projectile.frame = 18;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 35)
				Projectile.frame = 19;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 40)
				Projectile.frame = 20;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 45)
				Projectile.frame = 21;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 50)
				Projectile.frame = 22;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 55)
				Projectile.frame = 23;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 60)
				Projectile.frame = 0;


			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbBig != ShamanOrbBig.VOLCANO || player.GetModPlayer<OrchidModPlayer>().orbCountBig <= 0)
				Projectile.Kill();

			if (Projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidModPlayer>().orbCountBig;
				int offsetX = 7;

				if (nbOrb > 1)
				{
					startX = -30 - offsetX;
					startY = -30 - offsetX;
				}

				if (nbOrb > 3)
				{
					startX = -15 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 5)
				{
					startX = -0 - offsetX;
					startY = -40 - offsetX;
				}

				if (nbOrb > 7)
				{
					startX = +15 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 9)
				{
					startX = +30 - offsetX;
					startY = -30 - offsetX;
				}

				if (Projectile.damage != 0)
				{
					Projectile.damage = 0;
					startX = -30 - offsetX;
					startY = -30 - offsetX;
				}
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + player.width / 2 + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(50) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 0.7f;
			}

			if (Main.rand.Next(50) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 0.7f;
			}
		}
	}
}
