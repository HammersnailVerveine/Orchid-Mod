using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Large
{

	public class TerrariumScepterOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarium Orb");
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
			Main.projFrames[Projectile.type] = 8;
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

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 65)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 70)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 75)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 80)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 85)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 90)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 95)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 100)
				Projectile.frame = 0;

			if (player.GetModPlayer<OrchidShaman>().orbCountLarge < 5 || player.GetModPlayer<OrchidShaman>().orbCountLarge > 35 || player.GetModPlayer<OrchidShaman>().shamanOrbLarge != ShamanOrbLarge.TERRARIUM)
				Projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidShaman>().orbCountLarge;

			if (Projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidShaman>().orbCountLarge;
				int offsetX = 7;

				if (nbOrb > 4)
				{
					startX = -43 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 9)
				{
					startX = -30 - offsetX;
					startY = -48 - offsetX;
				}

				if (nbOrb > 14)
				{
					startX = -15 - offsetX;
					startY = -53 - offsetX;
				}

				if (nbOrb > 19)
				{
					startX = -0 - offsetX;
					startY = -55 - offsetX;
				}

				if (nbOrb > 24)
				{
					startX = +15 - offsetX;
					startY = -53 - offsetX;
				}

				if (nbOrb > 29)
				{
					startX = +30 - offsetX;
					startY = -48 - offsetX;
				}

				if (nbOrb > 34)
				{
					startX = +43 - offsetX;
					startY = -38 - offsetX;
				}

				if (Projectile.damage != 0)
				{
					Projectile.damage = 0;
					startX = -43 - offsetX;
					startY = -38 - offsetX;
				}
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + player.width / 2 + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.NextBool(20))
			{
				int dustType = Main.rand.Next(6) + 59;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Main.rand.Next(6) + 59;
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
