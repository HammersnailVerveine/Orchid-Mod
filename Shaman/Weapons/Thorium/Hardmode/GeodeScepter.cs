using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class GeodeScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 31;
			item.width = 50;
			item.height = 50;
			item.useTime = 42;
			item.useAnimation = 42;
			item.knockBack = 8f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("GeodeScepterProj");
			this.empowermentType = 4;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Scepter");
			Tooltip.SetDefault("Launches Heavy Geodes, exploding after a while"
							+ "\nThe explosion will release a burst of crystal shards"
							+ "\nThe more shamanic bonds you have, the more shards will appear");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "Geode", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

