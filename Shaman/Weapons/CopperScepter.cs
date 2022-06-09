using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class CopperScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 74;
			Item.useAnimation = 74;
			Item.knockBack = 3.25f;
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 4, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 6f;
			Item.shoot = Mod.Find<ModProjectile>("CopperScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an amethyst orb"
							  + "\nIf you have 3 amethyst orbs, your next hit will empower your shamanic spirit bonds for 15 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Amethyst, 8);
			recipe.AddIngredient(ItemID.CopperBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
