using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle
{
    public class GraniteEnergyScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 35;
			projectile.scale = 1f;
			projectile.tileCollide = true;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Surge");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			for(int i=0; i<2; i++)
			{
                float x = projectile.position.X - projectile.velocity.X / 10f * (float) i;
                float y = projectile.position.Y - projectile.velocity.Y / 10f * (float) i;
                int index2 = Dust.NewDust(new Vector2(x, y), projectile.width, projectile.height, 172, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].alpha = projectile.alpha;
			    Main.dust[index2].scale = (float) Main.rand.Next(70, 110) * 0.013f;
                Main.dust[index2].velocity *= 0.0f;
                Main.dust[index2].noGravity = true;
           }
	    }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("GraniteSurge")), 3 * 60);
			}
			
			if (Main.LocalPlayer.FindBuffIndex(mod.BuffType("GraniteAura")) > -1 || Main.LocalPlayer.FindBuffIndex(mod.BuffType("SpiritualBurst")) > -1)
			{
				return;
			}
			
			if (modPlayer.shamanOrbCircle != ShamanOrbCircle.GRANITE) {
				modPlayer.shamanOrbCircle = ShamanOrbCircle.GRANITE;
				modPlayer.orbCountCircle = 0;
			}
			modPlayer.orbCountCircle ++;
			
			if (modPlayer.orbCountCircle == 1) {
				Projectile.NewProjectile(player.Center.X, player.position.Y - 100, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			}
			
			if (modPlayer.orbCountCircle == 2)
				Projectile.NewProjectile(player.Center.X + 110, player.position.Y + 10, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			
			
			if (modPlayer.orbCountCircle == 3)
				Projectile.NewProjectile(player.Center.X, player.position.Y + 120, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			
			
			if (modPlayer.orbCountCircle == 4)
				Projectile.NewProjectile(player.Center.X - 110, player.position.Y + 10, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrb"), 0, 0, projectile.owner, 0f, 0f);

			
			if (modPlayer.orbCountCircle == 5) {
				modPlayer.orbCountCircle = 0;
				
				player.AddBuff(mod.BuffType("GraniteAura"), 60 * 30);
				Projectile.NewProjectile(player.Center.X, player.position.Y - 100, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrbProj"), 1, 0, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(player.Center.X + 110, player.position.Y + 10, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrbProj"), 2, 0, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(player.Center.X, player.position.Y + 120, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrbProj"), 3, 0, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(player.Center.X - 110, player.position.Y + 10, 0f, 0f, mod.ProjectileType("GraniteEnergyScepterOrbProj"), 4, 0, projectile.owner, 0f, 0f);
			}
		}
	}
}