using Terraria;

namespace OrchidMod.General.Items.Misc
{
	public class CopperKey : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens the Mineshaft Box");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 1;
			Item.useStyle = 0;
			Item.value = Item.sellPrice(0, 0, 5, 0);
		}
	}
}