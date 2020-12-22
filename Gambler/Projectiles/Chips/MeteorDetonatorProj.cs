using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class MeteorDetonatorProj : OrchidModGamblerProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			this.gamblingChipChance = 5;
        }
		
        public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			projectile.width = projectile.damage * 4;
			projectile.height = projectile.width;
			projectile.position.X = player.Center.X - projectile.width / 2;
			projectile.position.Y = player.Center.Y - projectile.width / 2;
			
			for (int i = 0 ; i < 20 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			OrchidModProjectile.spawnExplosionGore(projectile);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 6), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 3), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 2), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 6), 15, true, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 3), 15, true, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, (int)(projectile.width / 2), 15, true, 1.5f, 1f, 5f);
		}
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Explosion");
        }
    }
}