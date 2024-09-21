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
		}

		public override void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool charged)
		{
		}
	}
}
