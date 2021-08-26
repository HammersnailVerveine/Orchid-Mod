using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class ScepterofStarpower : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 38;
			item.height = 38;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 5.5f;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 15, 0);
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = ModContent.ProjectileType<Projectiles.StarpowerScepterProj>();
			item.crit = 4;
			empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scepter of Starpower");
			Tooltip.SetDefault("Critical strike chance increases with the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			// I hate it
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			item.crit = 4 + 10 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) + modPlayer.shamanCrit;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
