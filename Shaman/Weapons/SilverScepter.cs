using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SilverScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 24;
			item.width = 36;
			item.height = 38;
			item.useTime = 62;
			item.useAnimation = 62;
			item.knockBack = 4f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 7.5f;
			item.shoot = mod.ProjectileType("SilverScepterProj");
			this.empowermentType = 4;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you a sapphire orb"
							  + "\nIf you have 3 sapphire orbs, your next hit will increase your shamanic critical strike chance for 30 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Sapphire, 8);
			recipe.AddIngredient(ItemID.SilverBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
