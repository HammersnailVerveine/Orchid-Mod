using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class IceStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 0, 35, 75);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			GuardStacks = 1;
			FlagOffset = 4;
			AuraRange = 10;
			StandardDuration = 1500;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}
		public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => isNPC && isOwner;

		public override Color GetColor()
		{
			return new Color(106, 210, 255);
		}

		public override void NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			if (isLocalPlayer && reinforced)
			{
				standardStats.defense += 5;
			}
		}

		public override void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
			if (npc.knockBackResist > 0f)
			{
				npc.velocity.X *= 0.85f;
			}
		}
	}
}
