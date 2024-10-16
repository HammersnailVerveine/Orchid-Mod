using OrchidMod.Common;
using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shapeshifter
{
	public abstract class OrchidModShapeshifterProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter) { }
		public virtual void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) { }

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();

			SafeOnHitNPC(target, hit, damageDone, player, shapeshifter);
		}

		public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			SafeModifyHitNPC(target, ref modifiers);
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			if (!modTarget.ShapeshifterHit)
			{
				modTarget.ShapeshifterHit = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					var packet = Mod.GetPacket();
					packet.Write((byte)OrchidModMessageType.NPCHITBYCLASS);
					packet.Write((byte)target.whoAmI);
					packet.Write((byte)4);
					packet.Send();
				}
			}
		}
	}
}