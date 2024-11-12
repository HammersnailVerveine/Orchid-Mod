using Terraria;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianParryItem : OrchidModGuardianItem
	{
		public int InvincibilityDuration = 40; // Iframes duration after a parry

		public virtual void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info, Projectile anchor) { }
		public virtual void OnParryNPC(Player player, OrchidGuardian guardian, NPC npc, Player.HurtInfo info, Projectile anchor) { }
		public virtual void OnParryProjectile(Player player, OrchidGuardian guardian,  Projectile projectile, Player.HurtInfo info, Projectile anchor) { }
	}
}
