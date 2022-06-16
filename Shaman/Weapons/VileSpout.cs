using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class VileSpout : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 27, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = ModContent.ProjectileType<VileSpoutProj>();
			this.empowermentType = 1;
			this.energy = 6;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoVelocityReforge = true;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Spout");
			Tooltip.SetDefault("Shoots short ranged corruption beams"
							  + "\nThe weapon range scales with the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			Item.shootSpeed = 7f + 2f * nbBonds;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
