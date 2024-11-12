using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	internal class GuardianGlobalNPC : GlobalNPC
	{
		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (projectile.ModProjectile is OrchidModGuardianAnchor anchor && npc.ModNPC != null)
			{
				npc.ModNPC.OnHitByItem(anchor.Owner, anchor.Owner.HeldItem, hit, damageDone);
			}
		}
	}
}
