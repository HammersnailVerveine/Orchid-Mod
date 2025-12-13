using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModSystems;
using OrchidMod.Content.Guardian.Misc;
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

		public override void OnKill(NPC npc)
		{
			if (npc.GetGlobalNPC<OrchidGlobalNPC>().GuardianHit && !npc.SpawnedFromStatue && OrchidMiscModSystem.SlamDropCooldown >= 300 && !npc.CountsAsACritter)
			{ // Slam pickups drop logic (every 10 sec, not if there are more than 2 nearby slams)
				OrchidMiscModSystem.SlamDropCooldown = 0;
				int slamType = ModContent.ItemType<Slam>();
				int count = 0;
				foreach (Item item in Main.item)
				{
					if (item.type == slamType && item.Center.Distance(npc.Center) < 160f && item.active)
					{
						count++;
						if (count == 3)
						{
							break;
						}
					}
				}

				if (count < 3)
				{
					Item.NewItem(npc.GetSource_Death(), npc.getRect(), slamType);
				}
			}
		}
	}
}
