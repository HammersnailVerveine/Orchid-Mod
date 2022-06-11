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
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 6, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 6.5f;
			Item.shoot = Mod.Find<ModProjectile>("TinScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you a topaz orb"
							  + "\nIf you have 3 topaz orbs, your next hit will increase your armor for 30 seconds");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Topaz, 8);
			recipe.AddIngredient(ItemID.TinBar, 10);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
