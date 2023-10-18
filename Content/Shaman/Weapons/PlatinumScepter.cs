using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class PlatinumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 32;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 46;
			Item.useAnimation = 46;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9.5f;
			//Item.shoot = ModContent.ProjectileType<PlatinumScepterProj>();
			this.Element = 4;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Diamond Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a diamond orb"
							  + "\nIf you have 3 diamond orbs, your next hit will increase the duration of upcoming shamanic bonds for 30 seconds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Diamond, 8)
			.AddIngredient(ItemID.PlatinumBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
