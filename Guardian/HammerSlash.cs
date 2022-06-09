using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Guardian
{
	public class HammerSlash : OrchidModProjectile
	{
		public override void AltSetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.timeLeft = 7;
			Projectile.alpha = 128;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hammer Slash");
		}
		
		public override void AI()
		{
			if (Projectile.frame < 6) 
			{
				Projectile.frame ++;
			}
		}
	}
}