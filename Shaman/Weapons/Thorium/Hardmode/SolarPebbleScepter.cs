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
			Item.damage = 60;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("SolarPebbleScepterProj").Type;
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
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "SolarPebble", 8);
				recipe.AddIngredient(ItemID.LunarTabletFragment, 10);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

