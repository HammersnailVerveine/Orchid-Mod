using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class AmberScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 26;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.75f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			////Item.shoot = ModContent.ProjectileType<AmberScepterProj>();
			this.Element = 4;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Amber Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you an amber orb"
							  + "\nIf you have 3 amber orbs, your next hit will increase your maximum life for 30 seconds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Amber, 8)
			.AddIngredient(ItemID.FossilOre, 15)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
