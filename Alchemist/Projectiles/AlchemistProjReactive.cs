using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles
{
    public abstract class AlchemistProjReactive : OrchidModProjectile
    {	
		public int spawnTimeLeft = 0;
		public int killTimeLeft = 0;
	
		public virtual void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer) {}
		
		public virtual void SafeAI() {}
		
		public virtual void Despawn() {}
		
		public sealed override void AltSetDefaults() {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.alchemistProjectile = true;
			modProjectile.alchemistReactiveProjectile = true;
			modProjectile.baseCritChance = this.baseCritChance;
		}
		
		public override void AI()
        {
			if (projectile.timeLeft == 1) {
				Despawn();
				projectile.active = false;
			}
			this.killTimeLeft = projectile.timeLeft;
			SafeAI();
		}
		
		public override void Kill(int timeLeft)
        {
			if (this.killTimeLeft < this.spawnTimeLeft) {
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				SafeKill(timeLeft, player, modPlayer);
				Despawn();
			} else {
				return;
			}
		}
    }
}