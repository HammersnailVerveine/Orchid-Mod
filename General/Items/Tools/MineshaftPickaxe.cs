using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Tools
{
	public class MineshaftPickaxe : OrchidModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Can mine meteorite");
		}

		public override void SetDefaults() {
			item.CloneDefaults(ItemID.SilverPickaxe);
			item.damage = 6;
			item.melee = true;
			item.width = 34;
			item.height = 34;
			item.useTime = 19;
			item.useAnimation = 19;
			item.pick = 50;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
	}
}