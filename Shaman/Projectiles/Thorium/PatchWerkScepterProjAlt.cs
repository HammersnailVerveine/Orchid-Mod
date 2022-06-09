using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PatchWerkScepterProjAlt : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maggot");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 10;
			Projectile.aiStyle = 63;
			Projectile.friendly = true;
			Projectile.timeLeft = 180;
			Projectile.penetrate = 3;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void AI()
		{
			Projectile.frameCounter++;

			if (Projectile.frameCounter > 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 2)
			{
				Projectile.frame = 0;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = -3;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}
	}
}
