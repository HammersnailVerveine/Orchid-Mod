using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Shaman
{
    public abstract class OrchidModShamanProjectile : OrchidModProjectile
    {
		public int empowermentType = 0;
		public int empowermentLevel = 0;
		public int spiritPollLoad = 0;
		
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
		
		public sealed override void AltSetDefaults() {
			Player player = Main.LocalPlayer;
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.shamanProjectile = true;
			modProjectile.baseCritChance = player.inventory[player.selectedItem].crit ;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (target.type != NPCID.TargetDummy) {
				modPlayer.addShamanicEmpowerment(this.empowermentType, this.empowermentLevel);
				modPlayer.shamanPollSpirit += this.spiritPollLoad;
			}
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
    }
}