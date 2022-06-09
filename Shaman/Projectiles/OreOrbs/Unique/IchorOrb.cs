using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IchorOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 10;
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

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 2)
				Projectile.frame = 0;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 2 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 4)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 4 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 6)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 6 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 8)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 8 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 10)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 10 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 12)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 12 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 14)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 14 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 16)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 16 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 18)
				Projectile.frame = 8;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 18 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 20)
				Projectile.frame = 9;

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 20 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.ICHOR)
				Projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(13) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 162);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
			}
			if (Main.rand.Next(13) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
			}

		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust;
				switch (Main.rand.Next(3))
				{
					case 0:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						break;
					case 1:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 162);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 2f;
						Main.dust[dust].noGravity = true;
						break;
					case 2:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 228);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 1.75f;
						Main.dust[dust].noGravity = true;
						break;
				}
			}

			int dmg = (int)((30 + (5 * (orbsNumber))) * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
			int rainCount = (int)(orbsNumber / 2);
			for (int i = 0; i < rainCount; i++)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X - Main.rand.Next(2) + 1, -((float)(Main.rand.Next(3) + 3))).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X + Projectile.velocity.X, player.Center.Y - 125 + Projectile.velocity.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("IchorOrbRain").Type, dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
			Main.player[Projectile.owner].GetModPlayer<OrchidModPlayer>().orbCountUnique = 0;
		}
	}
}
