using OrchidMod.Shaman.Projectiles.OreOrbs.Small;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class TinScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 16;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 6, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 6.5f;
			Item.shoot = ModContent.ProjectileType<TinScepterProj>();
			this.Element = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Topaz Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a topaz orb"
							  + "\nIf you have 3 topaz orbs, your next hit will increase your armor for 30 seconds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Topaz, 8)
			.AddIngredient(ItemID.TinBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
