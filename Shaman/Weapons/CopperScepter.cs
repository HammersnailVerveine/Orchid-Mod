using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class CopperScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 14;
			item.width = 36;
			item.height = 38;
			item.useTime = 74;
			item.useAnimation = 74;
			item.knockBack = 3.25f;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 4, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 6f;
			item.shoot = mod.ProjectileType("CopperScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an amethyst orb"
							  + "\nIf you have 3 amethyst orbs, your next hit will empower your shamanic spirit bonds for 15 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Amethyst, 8);
			recipe.AddIngredient(ItemID.CopperBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
