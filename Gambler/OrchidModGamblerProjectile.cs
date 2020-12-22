using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Gambler
{
    public abstract class OrchidModGamblerProjectile : OrchidModProjectile
    {	
		public int gamblingChipChance = 0;
	
		public virtual void SafeAI() {}
		
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	
		public sealed override void AltSetDefaults() {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.gamblerProjectile = true;
			modProjectile.baseCritChance = this.baseCritChance;
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
			if (target.type != NPCID.TargetDummy && this.gamblingChipChance > 0) {
				modPlayer.addGamblerChip(this.gamblingChipChance);
			}
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
    }
}