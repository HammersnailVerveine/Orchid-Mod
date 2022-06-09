using Terraria;
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
			Item.rare = 3;
			Item.crit = 4;
			Item.accessory = true;
			Item.damage = 30;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Ring");
			Tooltip.SetDefault("Allows the wearer to release damaging droplets of lava while under the effect of shamanic air bond");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDripping = true;
		}
	}
}
