using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
    public class MonowaiProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 25;
			projectile.scale = 1f;
			projectile.alpha = 128;
			aiType = ProjectileID.Bullet; 
            this.empowermentType = 4;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 5;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcano Bolt");
        } 
		
        public override void AI()
        {  
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			Main.dust[dust].velocity = projectile.velocity / 3;
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
				
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59);
			Main.dust[dust].velocity = projectile.velocity / 3;
			Main.dust[dust2].scale = 1.5f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ ) {
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].velocity.Y = - 3f - (float)(Main.rand.Next(5));
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null && Main.rand.Next(2) == 0) {
				target.AddBuff((thoriumMod.BuffType("Singed")), 2 * 60);
			}
			
			if (modPlayer.shamanOrbBig != ShamanOrbBig.VOLCANO) {
				modPlayer.shamanOrbBig = ShamanOrbBig.VOLCANO;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig ++;
			
			if (modPlayer.orbCountBig == 2)
				{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 0, 0, projectile.owner, 0f, 0f);
				
				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountBig += 2;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountBig == 4)
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 40, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 8)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 10)
				Projectile.NewProjectile(player.Center.X + 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("MonowaiOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 10) {
				int dmg = (int)(50 * modPlayer.shamanDamage);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("MonowaiExplosion"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
				spawnDustCircle(6, 80);
				spawnDustCircle(59, 60);
				spawnDustCircle(6, 50);
				spawnDustCircle(59, 40);
				spawnDustCircle(6, 30);
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
	
				modPlayer.orbCountBig = -3;
			}
		}
    }
}