using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class GoldScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 28;
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
			//Item.shoot = ModContent.ProjectileType<GoldScepterProj>();
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Ruby Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a ruby orb"
							  + "\nIf you have 3 ruby orbs, your next hit will increase your life regeneration for 30 seconds"); */
		}
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Ruby, 8)
			.AddIngredient(ItemID.GoldBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
