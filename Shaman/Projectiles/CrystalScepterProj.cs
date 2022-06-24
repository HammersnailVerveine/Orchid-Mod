using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class CrystalScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Beam");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 40;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 100;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 40)
			{
				spawnDustCircle(Main.rand.NextBool(2) ? 185 : 62, 20);
			}
			else if (Projectile.timeLeft == 38)
			{
				spawnDustCircle(Main.rand.NextBool(2) ? 185 : 62, 15);
			}

			if (Main.rand.NextBool(40) && Projectile.timeLeft < 30 && Projectile.timeLeft > 10)
			{
				spawnDustCircle(Main.rand.NextBool(2) ? 185 : 62, 10);
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X / 3, Projectile.velocity.Y / 3).RotatedByRandom(MathHelper.ToRadians(360));
				int type = ModContent.ProjectileType<CrystalScepterProj2>();
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, perturbedSpeed, type, (int)(Projectile.damage * 0.6), (float)(Projectile.knockBack * 0.35), Projectile.owner, 0f, 0f);
			}

			if (Projectile.timeLeft == 0)
			{
				Player player = Main.player[Projectile.owner];
				OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
				int type = ModContent.ProjectileType<CrystalScepterProj2>();

				for (int i = 0; i < 2 + modPlayer.GetNbShamanicBonds(); i++)
				{
					Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X / 3, Projectile.velocity.Y / 3).RotatedByRandom(MathHelper.ToRadians(360));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, perturbedSpeed, type, (int)(Projectile.damage * 0.6), (float)(Projectile.knockBack * 0.35), Projectile.owner, 0f, 0f);
				}

				spawnDustCircle(185, 20);
				spawnDustCircle(62, 30);
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 185);
			Main.dust[dust].velocity = Projectile.velocity;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 62);
			Main.dust[dust2].velocity = Projectile.velocity;
			Main.dust[dust2].scale = 1f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double dustDeg = (i * (18)) + 5 - Main.rand.Next(10);
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = Projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity *= 0.05f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f;
				Main.dust[index1].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}