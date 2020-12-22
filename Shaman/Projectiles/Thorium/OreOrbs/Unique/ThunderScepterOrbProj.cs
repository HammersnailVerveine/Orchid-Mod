using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
    public class ThunderScepterOrbProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 16;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 100;	
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 6;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thunder Bolt");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 10 == 0)
				projectile.frame ++;
			if (projectile.frame == 6)
				projectile.frame = 0;
			
            if (Main.rand.Next(3) == 0)
			{   
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			    int dust = Dust.NewDust(pos, projectile.width, projectile.height, 229, 0f, 0f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 229);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
			
			Player player = Main.player[projectile.owner];
			int dmg = (int)(35 * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y + 10, 5f, 0f, mod.ProjectileType("ThunderScepterTornado"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y + 10, -5f, 0f, mod.ProjectileType("ThunderScepterTornado"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
        }
    }
}