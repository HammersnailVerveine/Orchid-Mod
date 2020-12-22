using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class ShroomKey : ModItem
	{
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Shroom Key");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 28;
			item.maxStack = 1;
			item.useStyle = 0;
			item.rare = 8;
			item.value = Item.sellPrice(0, 0, 0, 0);
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			string str = "";
			if (NPC.downedPlantBoss)
			{
				str = "Unlocks a Shroom Chest in the dungeon";
			} else {
				str = "It has been cursed by a powerful Jungle creature";
			}
			tooltips.Insert(1, new TooltipLine(mod, "KeyTag", str));
		}
	}
}