using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
    public class AquaiteScepterOrbProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 30;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 20;	
			projectile.scale = 1f;
			projectile.alpha = 192;
			projectile.tileCollide = false;
			projectile.penetrate = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geyser");
        } 
		
        public override void AI()
        {
		    for(int i=0; i<5; i++)
			{
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
				
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
				Main.dust[dust2].velocity /= 1f;
				Main.dust[dust2].scale = 1.7f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
           }
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
				target.velocity.Y = -15f;
				target.AddBuff((mod.BuffType("AquaBump")), 10 * 60);
			}
		}
    }
}