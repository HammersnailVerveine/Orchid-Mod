using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class QueenJellyfishScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sorta Fish Consisting of Gelatin");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 90;
			Projectile.penetrate = 4;
			Projectile.alpha = 126;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0f);
			Projectile.velocity.Y += 0.1f;

			if (Main.rand.Next(3) == 0)
			{
				int index2 = Dust.NewDust(Projectile.position - Projectile.velocity * 0.25f, Projectile.width, Projectile.height, 64, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
				Main.dust[index2].velocity = Projectile.velocity / 3;
				Main.dust[index2].noGravity = true;
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 1f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.3f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Projectile.damage += (2 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod));

			Projectile.penetrate--;
			Projectile.timeLeft = 90;
			spawnDustCircle(64, 20);
			if (Projectile.penetrate < 0) Projectile.Kill();
			Projectile.velocity.X = (Projectile.velocity.X != oldVelocity.X) ? -oldVelocity.X : Projectile.velocity.X * 0.8f;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 1.2f;
			SoundEngine.PlaySound(SoundID.Item87, Projectile.position);
			Projectile.netUpdate = true;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Projectile.Kill();
			spawnDustCircle(64, 20);
		}
	}
}