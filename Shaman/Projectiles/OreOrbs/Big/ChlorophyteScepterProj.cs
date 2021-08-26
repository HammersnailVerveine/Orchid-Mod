using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
    public class ChlorophyteScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 25;
			projectile.scale = 1f;
			projectile.alpha = 255;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Bolt");
        } 
		
        public override void AI()
        {  
            int dust = Dust.NewDust(projectile.Center, 1, 1, 61);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(projectile.Center, 1, 1, 44);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.3f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbBig != ShamanOrbBig.CHLOROPHYTE) {
				modPlayer.shamanOrbBig = ShamanOrbBig.CHLOROPHYTE;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig ++;
			//modPlayer.sendOrbCountPackets();
			
			if (modPlayer.orbCountBig == 3)
				{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 0, 0, projectile.owner, 0f, 0f);
				
				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountBig +=3;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
					//modPlayer.sendOrbCountPackets();
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X - 19, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(player.Center.X - 2, player.position.Y - 40, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(player.Center.X + 32, player.position.Y - 30, 0f, 0f, mod.ProjectileType("ChlorophyteOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15) {
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X, player.Center.Y - 50, perturbedSpeed.X, perturbedSpeed.Y, 228, projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
					
				perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X - 16, player.Center.Y - 48, perturbedSpeed.X, perturbedSpeed.Y, 228, projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
				
				perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X - 30, player.Center.Y - 40, perturbedSpeed.X, perturbedSpeed.Y, 228, projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
				
				perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X + 16, player.Center.Y - 48, perturbedSpeed.X, perturbedSpeed.Y, 228, projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
				
				perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X + 30, player.Center.Y - 40, perturbedSpeed.X, perturbedSpeed.Y, 228, projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
				modPlayer.orbCountBig = -3;
			}
		}
    }
}