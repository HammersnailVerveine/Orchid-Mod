using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class IceStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 35, 75);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			guardStacks = 1;
			flagOffset = 8;
			auraRange = 10;
			duration = 1800;
			affectNearbyPlayers = true;
			affectNearbyNPCs = true;
		}

		public override Color GetColor()
		{
			return new Color(106, 210, 255);
		}

		public override void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool charged)
		{
			player.statDefense += 5;
		}

		public override void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool charged)
		{
			if (npc.knockBackResist > 0f)
			{
				npc.velocity.X *= 0.85f;
			}
		}
	}
}
