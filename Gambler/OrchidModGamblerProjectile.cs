using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Gambler
{
    public abstract class OrchidModGamblerProjectile : OrchidModProjectile
    {	
		public int gamblingChipChance = 0;
		public bool bonusTrigger = false;
	
		public virtual void SafeAI() {}
		
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
		
		public virtual void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy = false) {}
	
		public sealed override void AltSetDefaults() {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			projectile.timeLeft = 1500;
			SafeSetDefaults();
			modProjectile.gamblerProjectile = true;
			modProjectile.baseCritChance = this.baseCritChance;
			modProjectile.gamblerBonusTrigger = this.bonusTrigger;
			modProjectile.gamblerBonusProjectilesDelegate = BonusProjectiles;
		}
		
		public override void AI() {
			this.SafeAI();
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			modProjectile.gamblerInternalCooldown -= modProjectile.gamblerInternalCooldown > 0 ? 1 : 0;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			if (target.type != NPCID.TargetDummy && this.gamblingChipChance > 0) {
				OrchidModGamblerHelper.addGamblerChip(this.gamblingChipChance, player, modPlayer);
			}
			modTarget.gamblerHit = true;
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
		
		public bool getDummy() {
			return projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
		}
    }
}