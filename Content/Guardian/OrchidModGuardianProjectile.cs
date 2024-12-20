using OrchidMod.Common;
using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian) { }
		public virtual void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) { }

		public sealed override void AltSetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			SafeSetDefaults();
		}

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();

			SafeOnHitNPC(target, hit, damageDone, player, guardian);
		}

		public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			SafeModifyHitNPC(target, ref modifiers);
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			if (!modTarget.GuardianHit)
			{
				modTarget.GuardianHit = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					var packet = Mod.GetPacket();
					packet.Write((byte)OrchidModMessageType.NPCHITBYCLASS);
					packet.Write((byte)target.whoAmI);
					packet.Write((byte)2);
					packet.Send();
				}
			}
		}
	}

	public abstract class OrchidModGuardianAnchor : OrchidModGuardianProjectile {} // Only used to "fake" true melee hits when hitting with those projectiles
}