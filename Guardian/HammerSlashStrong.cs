using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Guardian
{
	public class HammerSlashStrong : OrchidModProjectile
	{
		public override void AltSetDefaults()
		{
			Projectile.width = 102;
			Projectile.height = 96;
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