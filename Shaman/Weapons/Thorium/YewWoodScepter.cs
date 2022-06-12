using OrchidMod.Common.Interfaces;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class YewWoodScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 18;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 3f;
			Item.shoot = ModContent.ProjectileType<YewWoodScepterProj>();
			this.empowermentType = 5;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
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
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod.Find<ModTile>("ArcaneArmorFabricator").Type);
				recipe.AddIngredient(thoriumMod, "YewWood", 20);
				recipe.AddIngredient(ItemID.Amethyst, 2);
				recipe.Register();
			}
		}
	}
}

