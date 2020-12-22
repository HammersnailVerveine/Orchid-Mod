using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Wings)]
	public class AbyssalWings : OrchidModShamanEquipable
	{

		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Allows flight and slow fall");
		}

		public override void SafeSetDefaults() {
			item.width = 22;
			item.height = 20;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 10;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.wingTimeMax = 180;
			modPlayer.abyssalWings = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) {
			speed = 9f;
			acceleration *= 2.5f;
		}

		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(null, "AbyssFragment", 14);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}