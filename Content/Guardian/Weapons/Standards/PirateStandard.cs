using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class PirateStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 30;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			SlamStacks = 2;
			FlagOffset = 7;
			AuraRange = 15;
			StandardDuration = 2100;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}

		public override bool DrawAura(bool isPlayer, bool isNPC, bool isOwner, bool isReinforced) => isNPC && isOwner;

		public override Color GetColor()
		{
			return new Color(200, 200, 200);
		}

		public override void NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			if (isLocalPlayer && reinforced)
			{
				standardStats.guardianDamage += 0.1f;
			}
		}

		public override void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
			npc.AddBuff(BuffID.Midas, 30);
		}
	}
}
