using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Small
{
	public class AmberScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amber Bolt");
        } 
		public override void SafeSetDefaults()
		{
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 40;	
			projectile.scale = 1f;
            this.empowermentType = 4;
			this.projectileTrail = true;			
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {    
			if (Main.rand.Next(5) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
            }
			
			if (!this.initialized) {
				this.initialized = true;
				projectile.ai[0] = (float)(Main.rand.Next(9) - 4);
				projectile.netUpdate = true;
			}
			
			projectile.rotation += 0.1f;
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[0] / 5));
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].velocity *= 3f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			if (modPlayer.shamanOrbSmall != ShamanOrbSmall.AMBER) {
				modPlayer.shamanOrbSmall = ShamanOrbSmall.AMBER;
				modPlayer.orbCountSmall = 0;
			}
			modPlayer.orbCountSmall ++;
			
			if (modPlayer.orbCountSmall == 1) 
				{
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 20, 0f, 0f, mod.ProjectileType("AmberOrb"), 0, 0, projectile.owner, 0f, 0f);
				
				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountSmall ++;
					Projectile.NewProjectile(player.Center.X , player.position.Y - 25, 0f, 0f, mod.ProjectileType("AmberOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountSmall == 2)
				Projectile.NewProjectile(player.Center.X , player.position.Y - 25, 0f, 0f, mod.ProjectileType("AmberOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountSmall == 3)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 20, 0f, 0f, mod.ProjectileType("AmberOrb"), 0, 0, projectile.owner, 0f, 0f);

			if (modPlayer.orbCountSmall > 3) {
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(20, true);
				player.statLife += 20;
				modPlayer.orbCountSmall = 0;
			}
		}
    }
}