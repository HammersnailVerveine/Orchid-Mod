using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
    public class ThunderScepterTornado : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 100;	
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 6;
			projectile.alpha = 128;
			projectile.tileCollide = false;
			projectile.penetrate = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tornado");
        } 
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 5 == 0)
				projectile.frame ++;
			if (projectile.frame == 6)
				projectile.frame = 0;
			
            if (Main.rand.Next(3) == 0)
			{   
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			    int dust = Dust.NewDust(pos, projectile.width, projectile.height, 16, 0f, 0f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
        }
    }
}