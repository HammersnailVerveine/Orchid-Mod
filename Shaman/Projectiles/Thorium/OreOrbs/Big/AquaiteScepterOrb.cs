using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class AquaiteScepterOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquaite Orb");
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
				Projectile.frame = 0;

			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbBig != ShamanOrbBig.AQUAITE || player.GetModPlayer<OrchidModPlayer>().orbCountBig <= 0)
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
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
