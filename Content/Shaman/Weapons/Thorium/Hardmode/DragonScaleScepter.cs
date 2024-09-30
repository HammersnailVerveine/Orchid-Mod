using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class DragonScaleScepter : OrchidModShamanItem
	{
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
			//Item.shoot = ModContent.ProjectileType<DragonScaleScepterProj>();
			this.Element = ShamanElement.SPIRIT;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Draconic Wrath");
			/* Tooltip.SetDefault("Fires out a bolt of piercing dragon fire"
							+ "\nThe more shamanic bonds you have, the more enemies can be hit"); */
		}

		/*
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(ModContent.ItemType<Misc.RitualScepter>(), 1);
				recipe.AddIngredient(thoriumMod, "GreenDragonScale", 9);
				recipe.Register();
			}
		}
		*/
	}
}

