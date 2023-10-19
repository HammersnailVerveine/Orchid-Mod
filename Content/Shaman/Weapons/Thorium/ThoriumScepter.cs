using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 28, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = false;
			Item.shootSpeed = 10f;
			//Item.shoot = ModContent.ProjectileType<ThoriumScepterProj>();
			this.Element = ShamanElement.FIRE;
			this.catalystType = ShamanCatalystType.ROTATE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Thorium Scepter");
			/* Tooltip.SetDefault("Fires out a bolt of magic, dividing upon hitting a foe"
							+ "\nIf you have 3 or more active shamanic bonds, the bonus projectiles will home at nearby enemies"); */
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
			}
		}
	}
}

