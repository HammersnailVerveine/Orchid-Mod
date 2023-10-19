using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class LeadScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 22;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 62;
			Item.useAnimation = 62;
			Item.autoReuse = true;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 7.5f;
			//Item.shoot = ModContent.ProjectileType<OnyxScepterProj>();
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Onyx Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you an Onyx orb"
							+ "\nIf you have 3 onyx orbs, your next hit will give you 3 armor penetration for 30 seconds"); */
		}

		//TODO Thorium: Onyx removed, alongside other onyx content, including associated projectiles and buffs. Aquamarine is its replaacement
		/*
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "Onyx", 8);
				recipe.AddIngredient(ItemID.LeadBar, 10);
				recipe.Register();
			}
		}
		*/
	}
}
