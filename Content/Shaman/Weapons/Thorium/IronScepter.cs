using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class IronScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 19;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 72;
			Item.useAnimation = 72;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 7f;
			//Item.shoot = Mod.Find<ModProjectile>("OpalScepterProj").Type;
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Opal Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you an Opal orb"
							+ "\nIf you have 3 opal orbs, your next hit will increase your shamanic critical strike damage for 30 seconds"); */
		}

		/*
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "Opal", 8);
				recipe.AddIngredient(ItemID.IronBar, 10);
				recipe.Register();
			}
		}
		*/
	}
}
