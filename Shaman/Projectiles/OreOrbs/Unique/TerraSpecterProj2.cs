using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
    public class TerraSpecterProj2 : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 40;	
			projectile.scale = 1f;
            this.empowermentType = 5;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terric Magic");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			Lighting.AddLight(projectile.Center, 0.010f, 0.010f, 0f);
            for(int i=0; i<2; i++)
			{   
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			    int dust = Dust.NewDust(pos, projectile.width, projectile.height/2, 169, projectile.velocity.X * 0f, projectile.velocity.Y * 0f);
			    Main.dust[dust].noGravity = true;
			    Main.dust[dust].scale = 1f;
				Main.dust[dust].velocity /= 10f;
			    Main.dust[dust].noLight = true;
				
			    int dust2 = Dust.NewDust(pos, projectile.width, projectile.height/2, 259, projectile.velocity.X * 1f, projectile.velocity.Y * 1f);
			    Main.dust[dust2].noGravity = true;
			    Main.dust[dust2].scale = 1f;
				Main.dust[dust2].velocity /= 10f;
			    Main.dust[dust2].noLight = true;
				
				int dust3 = Dust.NewDust(pos, projectile.width, projectile.height/2, 246, projectile.velocity.X * 2f, projectile.velocity.Y * 2f);
			    Main.dust[dust3].noGravity = true;
			    Main.dust[dust3].scale = 1f;
				Main.dust[dust3].velocity /= 10f;
			    Main.dust[dust3].noLight = true;
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            return true;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.TERRA) {
				modPlayer.shamanOrbUnique = ShamanOrbUnique.TERRA;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique ++;
			//modPlayer.sendOrbCountPackets();
			
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("TerraScepterOrb1"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				//modPlayer.sendOrbCountPackets();
			}
			
			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("TerraScepterOrb1"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 10)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("TerraScepterOrb2"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 15)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("TerraScepterOrb3"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 20)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("TerraScepterOrb4"), 0, 0, projectile.owner, 0f, 0f);
		}
    }
}