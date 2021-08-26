using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using OrchidMod.Interfaces;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class TerrariumScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 94;
			item.width = 56;
			item.height = 56;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 3.5f;
			item.rare = ItemRarityID.Red;
			item.value = Item.sellPrice(0, 13, 50, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("TerrariumScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarium Scepter");
			Tooltip.SetDefault("Fires bolts of chromatic energy"
							+"\nHitting enemies will gradually grant you terrarium orbs"
							+"\nWhen reaching 7 orbs, they will break free and home into your enemies");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			var tt = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
			if (tt != null) tt.overrideColor = Main.DiscoColor;
		}
		
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.LunarCraftingStation);
				recipe.AddIngredient(thoriumMod, "TerrariumCore", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

