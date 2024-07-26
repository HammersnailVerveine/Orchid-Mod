using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using Terraria;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian) { }

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();

			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			modTarget.GuardianHit = true;

			SafeOnHitNPC(target, hit, damageDone, player, guardian);
		}
	}
}