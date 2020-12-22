using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
    public class WindBondProj2 : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;	
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 6;
			projectile.alpha = 128;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emerald Tornado");
        } 
		
        public override void AI()
        {
			Player player = Main.player[Main.myPlayer];
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 5 == 0)
				projectile.frame ++;
			if (projectile.frame == 6)
				projectile.frame = 0;
			
            if (Main.rand.Next(3) == 0)
			{   
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			    int dust = Dust.NewDust(pos, projectile.width, projectile.height, 61, 0f, 0f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
			}
			
			if (projectile.ai[1] > 0 ) {
				Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				float offsetX = player.Center.X - center.X;
				float offsetY = player.Center.Y - center.Y;
				float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
				if (distance < 50f && projectile.position.X < player.position.X + player.width && projectile.position.X + projectile.width > player.position.X 
				&& projectile.position.Y < player.position.Y + player.height && projectile.position.Y + projectile.height > player.position.Y) {
					player.AddBuff((mod.BuffType("WindSpeed")), 60 * 5);
				}
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
				if (projectile.ai[1] >= 2 && projectile.ai[1] < 3) {
					target.AddBuff(mod.BuffType("WindSlow"), 60 * 5);
				} else if (projectile.ai[1] >= 3 && projectile.ai[1] < 4) {
					target.AddBuff(mod.BuffType("WindStun"), 60 * 5);
				} else if (projectile.ai[1] >= 4) {
					target.AddBuff(mod.BuffType("WindDamage"), 60 * 5);
				}
			}
		}
    }
}