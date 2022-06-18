using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IchoryConeProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 50;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 50)
			{
				spawnDustCircle(Main.rand.Next(2) == 0 ? 259 : 162, 20);
				Projectile.timeLeft -= Main.rand.Next(20);
			}

			int dust = 169;
			for (int i = 0; i < 3; i++)
			{
				switch (Main.rand.Next(3))
				{
					case 0:
						dust = 259;
						break;
					case 1:
						dust = 162;
						break;
					case 2:
						dust = 64;
						break;
				}
				dust = Dust.NewDust(Projectile.Center, 1, 1, dust);
				Main.dust[dust].velocity = Projectile.velocity;
				Main.dust[dust].scale = 0.8f + ((Projectile.timeLeft) / 45f) * 1.8f;
				Main.dust[dust].noGravity = true;
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double dustDeg = (i * (36)) + 5 - Main.rand.Next(10);
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - Projectile.width / 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - Projectile.height / 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity = Projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f + ((Projectile.timeLeft) / 90f) * 1.3f; ;
				Main.dust[index1].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(15) == 0)
				target.AddBuff(69, 600);

			if (Projectile.penetrate == 3)
			{
				if (modPlayer.shamanOrbUnique != ShamanOrbUnique.ICHOR)
				{
					modPlayer.shamanOrbUnique = ShamanOrbUnique.ICHOR;
					modPlayer.orbCountUnique = 0;
				}
				modPlayer.orbCountUnique++;
				//modPlayer.sendOrbCountPackets();

				if (modPlayer.orbCountUnique == 1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("IchorOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
				{
					modPlayer.orbCountUnique += 5;
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
					//modPlayer.sendOrbCountPackets();
				}
			}
		}
	}
}