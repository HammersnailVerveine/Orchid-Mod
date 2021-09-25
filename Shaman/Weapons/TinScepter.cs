using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class TinScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 36;
			item.height = 38;
			item.useTime = 70;
			item.useAnimation = 70;
			item.knockBack = 3f;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 6, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 6.5f;
			item.shoot = mod.ProjectileType("TinScepterProj");
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Topaz, 8);
			recipe.AddIngredient(ItemID.TinBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
