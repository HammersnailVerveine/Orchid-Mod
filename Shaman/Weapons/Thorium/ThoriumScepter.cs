using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class ThoriumScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3.25f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 28, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = false;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("ThoriumScepterProj").Type;
			this.empowermentType = 1;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
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
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod.Find<ModTile>("ThoriumAnvil").Type);
				recipe.AddIngredient(thoriumMod, "ThoriumBar", 8);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

