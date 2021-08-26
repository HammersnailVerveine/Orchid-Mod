using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
    public class ValadiumScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 30;
			projectile.scale = 1f;
			projectile.alpha = 255;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Valadium Bolt");
        } 
		
        public override void AI()
        {
			projectile.velocity.Y -= 0.15f;
			
            int dust = Dust.NewDust(projectile.position, 1, 1, 70);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(projectile.position, 1, 1, 112);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 0.8f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
			spawnDustCircle(70, 10 + Main.rand.Next(16));
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 10 ; i ++ ) {
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null && Main.rand.Next(2) == 0) {
				target.AddBuff((thoriumMod.BuffType("LightCurse")), 1 * 60);
			}
			
			if (modPlayer.shamanOrbBig != ShamanOrbBig.VALADIUM) {
				modPlayer.shamanOrbBig = ShamanOrbBig.VALADIUM;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig ++;
			
			if (modPlayer.orbCountBig == 3)
				{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 0, 0, projectile.owner, 0f, 0f);
				
				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 40, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(player.Center.X + 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("ValadiumOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15) {
				knockback = 0f;
				if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
					target.velocity.X = projectile.velocity.X > 0f ? 8f : -8f;
					target.velocity.Y = -10f;
					target.AddBuff((mod.BuffType("AquaBump")), 10 * 60);
				}
				spawnDustCircle(70, 35);
				modPlayer.orbCountBig = -3;
			}
		}
    }
}