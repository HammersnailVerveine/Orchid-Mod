using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class DesertStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 25;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			SlamStacks = 1;
			FlagOffset = 8;
			AuraRange = 15;
			StandardDuration = 1200;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}
		public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isNPC && isOwner && isReinforced) || (isPlayer && !PlayerisOwner);

		public override Color GetColor()
		{
			return new Color(255, 249, 59);
		}

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			standardStats.moveSpeed += 0.1f;
			return true;
		}

		public override bool NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
			return true;
		}
	}
}
