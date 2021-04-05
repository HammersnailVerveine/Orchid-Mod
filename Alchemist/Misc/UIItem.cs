using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace OrchidMod.Alchemist.Misc
{
	public class UIItem : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.rare = 1;
			item.UseSound = SoundID.Item7;
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (player.altFunctionUse == 2) {
				return !modPlayer.alchemistSelectUIDisplay && Main.mouseRightRelease;
			} // else {
				// return !modPlayer.alchemistSelectUIDisplay && Main.mouseLeftRelease;
			// }
			return base.CanUseItem(player);
		}
		
		public override bool UseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!modPlayer.alchemistSelectUIDisplay && Main.mouseLeftRelease) {
				modPlayer.alchemistSelectUIDisplay = true;
				modPlayer.alchemistSelectUIInitialize = modPlayer.alchemistSelectUIDisplay ? true : false;
			}
			return true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}
		
		public override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistSelectUIItem = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Alchemist's Cookbook");
			Tooltip.SetDefault("Allows to mix alchemical weapons by clicking"
							+  "\nRight click on an item icon to mix it"
							+  "\nLeft click to launch the attack"
							+  "\nUp to 18 items can be displayed at once");
		}

	}
}
