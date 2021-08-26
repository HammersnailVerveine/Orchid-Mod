using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }

		public sealed override void AltSetDefaults()
		{
			Player player = Main.LocalPlayer;
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.shamanProjectile = true;
			modProjectile.baseCritChance = player.inventory[player.selectedItem].crit;
		}

		public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (target.type != NPCID.TargetDummy)
			{
				OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				OrchidModShamanHelper.addShamanicEmpowerment(modProjectile.shamanEmpowermentType, player, modPlayer, mod);
			}
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
	}
}