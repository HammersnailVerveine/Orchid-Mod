using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class IceChestFlaskProjSmall : OrchidModAlchemistProjectile
    {
        public override void SafeSetDefaults() {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.penetrate = 1;
			projectile.friendly = true;
			Main.projFrames[projectile.type] = 3;
			this.projectileTrail = true;
			projectile.alpha = 128;
        }
		
		// public override Color? GetAlpha(Color lightColor) {
            // return Color.White;
        // }
		
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flash Freeze Flake");
        }
		
        public override void AI() {
			projectile.velocity.Y += 0.05f;
			projectile.rotation += (projectile.velocity.Y * 1.5f) / 20f;
		}
		
		public override void Kill(int timeLeft) {
			int range = 50;
			// OrchidModProjectile.spawnDustCircle(projectile.Center, 67, (int)(range / 2), 5, true, 1.5f, 1f, 6f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 67, 5, 6, true, 1f, 1f, 4f);
			spawnGenericExplosion(projectile, projectile.damage, 1f, range * 2, 2, false, 27);
		}
    }
}