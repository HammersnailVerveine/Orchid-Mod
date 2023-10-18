using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class GeodeScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 31;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.knockBack = 8f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<GeodeScepterProj>();
			this.Element = 4;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Geode Scepter");
			/* Tooltip.SetDefault("Launches Heavy Geodes, exploding after a while"
							+ "\nThe explosion will release a burst of crystal shards"
							+ "\nThe more shamanic bonds you have, the more shards will appear"); */
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "Geode", 8);
				recipe.Register();
			}
		}
	}
}

