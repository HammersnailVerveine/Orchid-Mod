using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class ThoriumScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 14;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.knockBack = 3.25f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 28, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("ThoriumScepterProj");
			this.empowermentType = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorium Scepter");
			Tooltip.SetDefault("Fires out a bolt of magic, dividing upon hitting a foe"
							+ "\nIf you have 3 or more active shamanic bonds, the bonus projectiles will home at nearby enemies");
		}
		
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(thoriumMod.TileType("ThoriumAnvil"));		
				recipe.AddIngredient(thoriumMod, "ThoriumBar", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

