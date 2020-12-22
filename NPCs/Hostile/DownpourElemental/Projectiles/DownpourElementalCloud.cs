using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.NPCs.Hostile.DownpourElemental.Projectiles
{
    public class DownpourElementalCloud : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.hostile = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 60;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thunder Cloud");
        }
		
        public override void AI()
        {
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 1f;
		}
    }
}