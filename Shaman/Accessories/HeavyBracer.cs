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
            item.width = 28;
            item.height = 24;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 4;
            item.accessory = true;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Heavy Bracelet");
		  Tooltip.SetDefault("An active shamanic earth bond will increase your armor by 10 but reduce your movement speed by 20%");
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanHeavy = true;
        }
    }
}
