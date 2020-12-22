using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
    public class WindBondProj1 : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 180;	
			projectile.scale = 1f;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wind Bolt");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			projectile.velocity *= 1.1f;
			
			Player player = Main.player[projectile.owner];		
            if (Main.rand.Next(3) == 0)
			{   
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			    int dust = Dust.NewDust(pos, projectile.width, projectile.height, 61, 0f, 0f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
			}
			
			if (projectile.timeLeft == 180) {
				for (int i = 0 ; i < 20 ; i ++) {
					Vector2 pos = new Vector2(projectile.position.X - 50 + projectile.width / 2, projectile.position.Y);
					int dust = Dust.NewDust(pos, 100, 50, 61, 0f, 0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 1.5f;
				}
				
				for (int i = 0 ; i < 20 ; i ++) {
					Vector2 pos = new Vector2(projectile.position.X - 75 + projectile.width / 2, projectile.position.Y);
					int dust = Dust.NewDust(pos, 150, 20, 61, 0f, 0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 1.5f;
				}
				
				for (int i = 0 ; i < 20 ; i ++) {
					Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
					int dust = Dust.NewDust(pos, projectile.width, projectile.height, 61, 0f, 0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity *= 7f;
				}
			}

        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
			
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int level = (modPlayer.shamanAirBuff + modPlayer.shamanAirBonus);
			int dmg = (int)(10 * level * modPlayer.shamanDamage + 5E-06f);
			float ai = projectile.ai[1];
			
			int airProj1 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y + 10, 7f, 0f, mod.ProjectileType("WindBondProj2"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			int airProj2 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y + 10, -7f, 0f, mod.ProjectileType("WindBondProj2"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			Main.projectile[airProj1].ai[1] = ai;
			Main.projectile[airProj1].netUpdate = true;
			Main.projectile[airProj2].ai[1] = ai;
			Main.projectile[airProj2].netUpdate = true;
        }
    }
}