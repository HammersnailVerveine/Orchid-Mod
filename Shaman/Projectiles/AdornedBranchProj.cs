using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class AdornedBranchProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Splinter");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 5;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 100;	
			projectile.extraUpdates = 1;
		}
		
		public override void AI()
		{
			if (projectile.timeLeft == 100 || projectile.timeLeft == 1 ) {	
				int dustType = 31;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}