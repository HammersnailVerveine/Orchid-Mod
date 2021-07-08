using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class BlumProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blum Bolt");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.penetrate = 3;
			projectile.CloneDefaults(123);
			aiType = 123;
			projectile.timeLeft = 23;
            this.empowermentType = 2;
		}
		
		public override bool PreKill(int timeLeft)
		{
			projectile.type = 123;
			return true;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}