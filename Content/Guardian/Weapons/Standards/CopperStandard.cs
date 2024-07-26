using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class CopperStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 3, 65);
			Item.rare = ItemRarityID.White;
			Item.useTime = 45;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			slamStacks = 1;
			flagOffset = 6;
			auraRange = 10;
			duration = 600;
			affectNearbyPlayers = true;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(106, 210, 255);
		}

		public override void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool charged)
		{
			player.statDefense += 3;
			if (isLocalPlayer && charged) player.statDefense += 3;
		}
	}
}
