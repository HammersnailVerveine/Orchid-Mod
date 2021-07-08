using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class MeteorPhasestaffProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.penetrate = 100;
			projectile.timeLeft = 60;
            projectile.extraUpdates = 5;
			projectile.ignoreWater = true;   
			projectile.alpha = 255;
            this.empowermentType = 1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasebeam");
        }
		
		public override void AI()
		{
			projectile.damage -= projectile.timeLeft % 5 == 0 && projectile.damage > 1 ? 1 : 0;
			
            int index1 = Dust.NewDust(projectile.position, 1, 1, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
            Main.dust[index1].velocity *= 0.2f;
			Main.dust[index1].fadeIn = 1f;
			Main.dust[index1].scale = 0.8f + ((projectile.timeLeft)/90f) * 1.8f;
            Main.dust[index1].noGravity = true;
			
			if (projectile.timeLeft == 60 || projectile.timeLeft == 55) {
				int dist = projectile.timeLeft == 55 ? 15 : 20;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 170, dist, 15, true, 1f, 0f);
			}
        }
		
		public override void Kill(int timeLeft)
        {
			OrchidModProjectile.spawnDustCircle(projectile.Center, 170, 20, 15, true, 1f, 0f);
            int dust = Dust.NewDust(projectile.position, 1, 1, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.2f;
			Main.dust[dust].scale = 2.5f;
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			target.AddBuff((24), 1 * 60);
		}
    }
}