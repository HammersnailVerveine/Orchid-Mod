using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class IceStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			GuardStacks = 1;
			FlagOffset = 4;
			AuraRange = 8;
			StandardDuration = 1500;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}
		public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => isNPC && isOwner;

		public override Color GetColor()
		{
			return new Color(106, 210, 255);
		}

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			if (isLocalPlayer && reinforced)
			{
				guardian.modPlayer.OrchidDamageResistance += 0.2f;
				return true;
			}
			return false;
		}

		public override bool NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
			if (npc.knockBackResist > 0f && !npc.HasBuff<HockeyQuarterstaffDebuff>())
			{
				npc.velocity *= 1f - 0.2f * npc.knockBackResist;
			}
			return true;
		}
	}
}
