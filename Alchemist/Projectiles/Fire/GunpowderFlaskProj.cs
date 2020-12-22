using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
    public class GunpowderFlaskProj : AlchemistProjCatalyst
    {
        public override void SetDefaults()
        {
            projectile.width = 250;
            projectile.height = 250;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
		public override void CatalystInteractionEffect(Player player) {}
		
        public override void SafeAI()
        {
			for (int i = 0 ; i < 20 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			OrchidModProjectile.spawnExplosionGore(projectile);
		}
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }
    }
}