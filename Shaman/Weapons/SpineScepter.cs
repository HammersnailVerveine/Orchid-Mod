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
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 27, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = Mod.Find<ModProjectile>("SpineScepterProj").Type;
			this.empowermentType = 2;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoVelocityReforge = true;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Spine Scepter");
			Tooltip.SetDefault("Shoots short ranged crimson beams"
							  + "\nThe weapon range scales with the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			Item.shootSpeed = 7f + 2f * nbBonds;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
