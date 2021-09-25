using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class SolarPebbleScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 60;
			item.width = 48;
			item.height = 48;
			item.useTime = 12;
			item.useAnimation = 12;
			item.knockBack = 4.25f;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("SolarPebbleScepterProj");
			this.empowermentType = 1;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 4;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ecliptic Flare");
			Tooltip.SetDefault("Fires a storm of solar embers"
							+ "\nHitting will charge an eclipse above you, releasing homing flames when full");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "SolarPebble", 8);
				recipe.AddIngredient(ItemID.LunarTabletFragment, 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

