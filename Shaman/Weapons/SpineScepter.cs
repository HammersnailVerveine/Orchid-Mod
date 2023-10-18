using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SpineScepter : OrchidModShamanItem
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
			//Item.shoot = ModContent.ProjectileType<SpineScepterProj>();
			this.Element = 2;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoVelocityReforge = true;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Spine Scepter");
			/* Tooltip.SetDefault("Shoots short ranged crimson beams"
							  + "\nThe weapon range scales with the number of active shamanic bonds"); */
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			Item.shootSpeed = 7f + 2f * nbBonds;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
