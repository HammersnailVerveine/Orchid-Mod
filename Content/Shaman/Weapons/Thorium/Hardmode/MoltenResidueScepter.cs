using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class MoltenResidueScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 56;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 1f;
			//Item.shoot = ModContent.ProjectileType<MoltenResidueScepterProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Molten Bomb");
			/* Tooltip.SetDefault("Fires out a magmatic bomb"
							+ "\nThe explosion size and damage depends on your number of active shamanic bonds"); */
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
				recipe.AddIngredient(thoriumMod, "SoulofPlight", 8);
				recipe.AddIngredient(ItemID.SoulofNight, 7);
				recipe.Register();
			}
		}
		*/
	}
}

