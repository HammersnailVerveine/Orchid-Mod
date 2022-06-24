using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }

		public sealed override void AltSetDefaults()
		{
			Player player = Main.LocalPlayer;
			OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.shamanProjectile = true;
			modProjectile.baseCritChance = player.inventory[player.selectedItem].crit;
		}

		public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			if (target.type != NPCID.TargetDummy)
			{
				OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modPlayer.AddShamanicEmpowerment(modProjectile.shamanEmpowermentType);
			}
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
	}
}