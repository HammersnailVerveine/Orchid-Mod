using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class UnfathomableFleshScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 50;
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
			//Item.shoot = ModContent.ProjectileType<UnfathomableFleshScepterProj>();
			this.Element = ShamanElement.SPIRIT;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Symbiosis Catalyst");
			/* Tooltip.SetDefault("Fires out a bolt of flesh magic"
							+ "\nIf you have 5 active shamanic bonds, your attack will steal life"
							+ "\nAfter stealing life, your regeneration will be nullified for a moment"); */
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
				recipe.AddIngredient(thoriumMod, "UnfathomableFlesh", 9);
				recipe.Register();
			}
		}
		*/
	}
}

