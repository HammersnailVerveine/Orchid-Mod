using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SilverScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 62;
			Item.useAnimation = 62;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 7.5f;
			//Item.shoot = ModContent.ProjectileType<SilverScepterProj>();
			this.Element = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Sapphire Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a sapphire orb"
							  + "\nIf you have 3 sapphire orbs, your next hit will increase your shamanic critical strike chance for 30 seconds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Sapphire, 8)
			.AddIngredient(ItemID.SilverBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
