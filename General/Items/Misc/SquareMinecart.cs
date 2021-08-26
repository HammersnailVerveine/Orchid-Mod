using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class SquareMinecart : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Square Minecart");
			Tooltip.SetDefault("'Great for impersonating Orchid Devs!'"); // S-Pladison
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 22;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.mountType = ModContent.MountType<Mounts.SquareMinecartMount>();
		}
	}
}
