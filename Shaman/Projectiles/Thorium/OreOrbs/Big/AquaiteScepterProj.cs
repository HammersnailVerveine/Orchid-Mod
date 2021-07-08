using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
    public class AquaiteScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 20;
			projectile.scale = 1f;
			projectile.alpha = 128;
			aiType = ProjectileID.Bullet; 
            this.empowermentType = 4;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquaite Bolt");
        } 
		
        public override void AI()
        {  
		
		    for(int i=0; i<2; i++)
			{
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
				
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbBig != ShamanOrbBig.AQUAITE) {
				modPlayer.shamanOrbBig = ShamanOrbBig.AQUAITE;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig ++;
			
			if (modPlayer.orbCountBig == 2)
				{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
				
				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountBig +=2;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountBig == 4)
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 40, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 8)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 10)
				Projectile.NewProjectile(player.Center.X + 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("AquaiteScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 10) {
				int dmg = (int)(50 * modPlayer.shamanDamage);
				Projectile.NewProjectile(target.position.X + (target.width / 2), target.position.Y + target.height + 4, 0f, -7.5f, mod.ProjectileType("AquaiteScepterOrbProj"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
				modPlayer.orbCountBig = -3;
			}
		}
    }
}