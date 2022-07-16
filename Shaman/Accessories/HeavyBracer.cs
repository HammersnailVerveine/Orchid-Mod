using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class HeavyBracer : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Heavy Bracelet");
			Tooltip.SetDefault("An active shamanic earth bond will increase your armor by 20 but reduce your movement speed by 20%");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanHeavy = true;
		}
	}
}
