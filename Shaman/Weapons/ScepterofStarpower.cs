using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class ScepterofStarpower : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shootSpeed = 9.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.StarpowerScepterProj>();
			Item.crit = 4;
			empowermentType = 3;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Scepter of Starpower");
			Tooltip.SetDefault("Critical strike chance increases with the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			// I hate it
			// I'm sorry :(
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			Item.crit = 4 + 10 * modPlayer.GetNbShamanicBonds() + modPlayer.shamanCrit; // [CRIT]
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.FallenStar, 5)
			.AddIngredient(ItemID.Wood, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
