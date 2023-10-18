using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Projectiles
{
	public class AlchemistSmoke2 : OrchidModAlchemistProjectile
	{
		public Color glowColor = new Color(255, 255, 255);
		public float rotationspeed = 0f;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemical Smoke");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.alpha = 64;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			// return this.glowColor;
			return Main.myPlayer == Projectile.owner ? this.glowColor : Color.White;
		}

		public override void AI()
		{
			Projectile.scale *= 0.98f;
			Projectile.velocity *= 0.95f;
			this.rotationspeed *= 0.99f;
			Projectile.alpha += 1 + Main.rand.Next(2);
			Projectile.rotation += this.rotationspeed;
			if (Projectile.timeLeft == 180)
			{
				this.glowColor.R = (byte)Projectile.localAI[0];
				this.glowColor.G = (byte)Projectile.localAI[1];
				this.glowColor.B = (byte)Projectile.ai[1];
				this.rotationspeed = (float)((1 + Main.rand.Next(3)) / 10f);
				this.rotationspeed *= Main.rand.NextBool(2) ? -1 : 1;
			}
			this.glowColor.A = (byte)(255 - Projectile.alpha);
			if (Projectile.alpha >= 196)
			{
				Projectile.Kill();
			}
			if (Main.rand.NextBool(30))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.WhiteDust>(), 0.0f, 0.0f, 0, this.glowColor);
				Main.dust[dust].velocity *= 0.5f; ;
				Main.dust[dust].noGravity = true;
			}
		}

		// public override bool OnTileCollide(Vector2 oldVelocity)
		// {
		// if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
		// if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
		// projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
		// return false;
		// }

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.WhiteDust>(), 0.0f, 0.0f, 0, this.glowColor);
				Main.dust[dust].velocity *= 0.5f; ;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}