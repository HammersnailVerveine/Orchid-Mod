using Terraria;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class GuardianTest : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = -11;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianInfiniteResources = true;
			if (!hideVisual) modPlayer.GuardianShowDebugVisuals = true;
		}

		public override void UpdateVanity(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianShowDebugVisuals = true;
		}
	}
}