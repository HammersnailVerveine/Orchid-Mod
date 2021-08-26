using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class YewWoodScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.knockBack = 3f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("YewWoodScepterProj");
			this.empowermentType = 1;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Scepter");
			Tooltip.SetDefault("Fires inaccurate bolts of shadowflame magic"
							+ "\nIf you have 3 or more bonds, hitting has a chance to summon a shadow portal");
		}
		
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(thoriumMod.TileType("ArcaneArmorFabricator"));		
				recipe.AddIngredient(thoriumMod, "YewWood", 20);
				recipe.AddIngredient(ItemID.Amethyst, 2);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

