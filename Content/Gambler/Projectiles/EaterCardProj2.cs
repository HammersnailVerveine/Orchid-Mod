using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class EaterCardProj2 : OrchidModGamblerProjectile
	{
		public static Texture2D outlineTexture;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Eater Flesh");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 255;
		}

		public override void SafeAI()
		{
			if (Projectile.owner != Main.myPlayer)
			{
				Projectile.active = false;
			}

			if (!this.Initialized)
			{
				if (Projectile.timeLeft < 1440)
				{
					this.Initialized = true;
					Projectile.velocity *= 0f;
					Projectile.alpha = 0;
					Projectile.damage = 1;
					for (int i = 0; i < 4; i++)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
				}
				else
				{
					Projectile.velocity.Y += 0.1f;
					if (Main.rand.NextBool(3))
					{
						int dust = Dust.NewDust(Projectile.Center, 1, 1, 18);
						Main.dust[dust].velocity *= 0f;
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/EaterCardProj2_Outline", AssetRequestMode.ImmediateLoad).Value;
			DrawOutline(outlineTexture, spriteBatch, lightColor);
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			if (timeLeft < 1499)
			{
				for (int i = 0; i < 4; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
			}
		}
	}
}