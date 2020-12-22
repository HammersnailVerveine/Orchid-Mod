using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Gambler.Armors.Outlaw
{
	[AutoloadEquip(EquipType.Legs)]
    public class OutlawPants : OrchidModGamblerEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 5, 0);
            item.rare = 1;
            item.defense = 2;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw Pants");
			Tooltip.SetDefault("4% increased gambling damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerDamage += 0.04f;
        }
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			Mod orchidMod = ModLoader.GetMod("OrchidMod");
			
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : orchidMod.ItemType("VultureTalon"), 3);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : orchidMod.ItemType("VultureTalon"), 3);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
