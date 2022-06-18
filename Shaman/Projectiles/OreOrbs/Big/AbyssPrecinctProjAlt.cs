using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class AbyssPrecinctProjAlt : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 89;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.alpha = 192;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;

			if (Projectile.timeLeft % 30 == 0)
			{
				spawnDustCircle(172, 50);
				spawnDustCircle(172, 100);
				spawnDustCircle(29, 75);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("AbyssPrecinctProjExplosion").Type, Projectile.damage * 2, 0.0f, Projectile.owner, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14);
				if (Projectile.timeLeft == 30) Projectile.Kill();
			}

			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = distToCenter == 50 ? Projectile.velocity : Projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}