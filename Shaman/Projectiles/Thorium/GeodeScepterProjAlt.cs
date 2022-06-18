using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class GeodeScepterProjAlt : OrchidModShamanProjectile
	{
		public int crystalType;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 90;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 3;
			this.crystalType = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Shard");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.friendly = Projectile.velocity.Y > 0;

			if (this.crystalType == 0)
			{
				this.crystalType = Main.rand.Next(3) + 1;
				Projectile.frame = this.crystalType - 1;
			}

			int dustType = 60;
			switch (this.crystalType)
			{
				case 1:
					dustType = 60;
					Lighting.AddLight(Projectile.position, 0.5f, 0f, 0f);
					break;
				case 2:
					dustType = 59;
					Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
					break;
				case 3:
					dustType = 62;
					Lighting.AddLight(Projectile.position, 0.25f, 0f, 0.25f);
					break;
				default:
					break;
			}

			if (Main.rand.Next(5) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].scale *= 1.5f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];

			int dustType = 60;
			switch (this.crystalType)
			{
				case 0:
					dustType = 60;
					break;
				case 1:
					dustType = 59;
					break;
				case 2:
					dustType = 62;
					break;
				default:
					break;
			}

			spawnDustCircle(dustType, 10);

			SoundEngine.PlaySound(SoundID.Item10);
			// Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("StarScouterScepterProjAltExplosion"), projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double dustDeg = (double)(i * (36));
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - Projectile.width / 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - Projectile.height / 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity *= 0.05f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f;
				Main.dust[index1].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}