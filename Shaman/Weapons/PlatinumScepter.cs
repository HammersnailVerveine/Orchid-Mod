using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class PlatinumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 32;
			item.width = 36;
			item.height = 38;
			item.useTime = 46;
			item.useAnimation = 46;
			item.knockBack = 5.5f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = mod.ProjectileType("PlatinumScepterProj");
			this.empowermentType = 4;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you a diamond orb"
							  + "\nIf you have 3 diamond orbs, your next hit will increase the duration of upcoming shamanic bonds for 30 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Diamond, 8);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
