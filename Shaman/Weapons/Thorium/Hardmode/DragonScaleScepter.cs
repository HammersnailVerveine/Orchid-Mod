using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class DragonScaleScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 45;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("DragonScaleScepterProj").Type;
			this.empowermentType = 5;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Draconic Wrath");
			Tooltip.SetDefault("Fires out a bolt of piercing dragon fire"
							+ "\nThe more shamanic bonds you have, the more enemies can be hit");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(Mod.Find<ModItem>("RitualScepter").Type, 1);
				recipe.AddIngredient(thoriumMod, "GreenDragonScale", 9);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

