using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class MeltedRing : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.damage = 30;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Ring");
			Tooltip.SetDefault("Allows the wearer to release damaging droplets of lava while under the effect of shamanic air bond");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanDripping = true;
		}
	}
}
