using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
    public class IceSpearScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 30;
			projectile.penetrate = 1;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 4;
            this.empowermentType = 2;
            this.empowermentLevel = 1;
            this.spiritPollLoad = 3;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Spear");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {  
			Player player = Main.player[projectile.owner];
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 3 == 0)
				projectile.frame ++;
			if (projectile.frame == 4)
				projectile.frame = 0;
			
			if (projectile.timeLeft == 30) {
				for(int i=0; i<3; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
					Main.dust[dust].velocity = - projectile.velocity / 5;
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = false;
			   }
			}
			
		    if (Main.rand.Next(6) == 0)
			{
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
           }
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.ICE) {
				modPlayer.shamanOrbUnique = ShamanOrbUnique.ICE;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique ++;
			//modPlayer.sendOrbCountPackets();
			
			if (modPlayer.orbCountUnique == 3)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("IceSpearOrb"), 0, 0, projectile.owner, 0f, 0f);
			
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 3;
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("IceSpearOrb"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				//modPlayer.sendOrbCountPackets();
			}
			
			if (modPlayer.orbCountUnique == 10) {	
			
				modPlayer.orbCountUnique = 0;
				
				float speedX = 1f;
				int angle  = 22;
				int dmg = (int)(projectile.damage + 35);
				
				if (projectile.velocity.X < 0) 
				{
					speedX = - speedX;
					angle = - angle;
				}
				
				Vector2 spearVelocity = ( new Vector2(speedX, 0f).RotatedBy(MathHelper.ToRadians(angle)));
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, spearVelocity.X, spearVelocity.Y, mod.ProjectileType("IceSpearProj"), dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
		}
    }
}