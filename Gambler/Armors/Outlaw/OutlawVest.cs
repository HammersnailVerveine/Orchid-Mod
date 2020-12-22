using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Gambler.Armors.Outlaw
{
	[AutoloadEquip(EquipType.Body)]
    public class OutlawVest : OrchidModGamblerEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 22;
            item.value = Item.sellPrice(0, 0, 6, 0);
            item.rare = 1;
            item.defense = 3;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw Vest");
			Tooltip.SetDefault("4% increased gambling damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerDamage += 0.04f;
        }
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			Mod orchidMod = ModLoader.GetMod("OrchidMod");
			
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 7);
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : orchidMod.ItemType("VultureTalon"), 4);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 7);
			recipe.AddIngredient(ItemID.PlatinumBar, 15);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : orchidMod.ItemType("VultureTalon"), 4);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
