using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Small
{

	public class CrimsonOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Orb");
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

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 65 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 0)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 70 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 5)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 75 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 10)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 80 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 15)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 85 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 20)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 90 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 25)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 95 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 30)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 100 || player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 35)
				Projectile.frame = 0;


			if (player.GetModPlayer<OrchidShaman>().shamanOrbSmall != ShamanOrbSmall.CRIMSON || player.GetModPlayer<OrchidShaman>().orbCountSmall == 0)
				Projectile.Kill();

			if (Projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidShaman>().orbCountSmall;
				int offsetX = 7;

				if (nbOrb == 1)
				{
					startX = -15 - offsetX;
					startY = -20 - offsetX;
				}

				if (nbOrb == 2)
				{
					startX = +0 - offsetX;
					startY = -25 - offsetX;
				}

				if (nbOrb == 3)
				{
					startX = +15 - offsetX;
					startY = -20 - offsetX;
				}

				if (Projectile.damage != 0)
				{
					Projectile.damage = 0;
					startX = -15 - offsetX;
					startY = -20 - offsetX;
				}
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + player.width / 2 + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(20) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 90);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 90);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
