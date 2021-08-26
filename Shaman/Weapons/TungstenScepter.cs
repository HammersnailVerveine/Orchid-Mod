using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class TungstenScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 28;
			item.width = 36;
			item.height = 38;
			item.useTime = 58;
			item.useAnimation = 58;
			item.knockBack = 4.25f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("TungstenScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an emerald orb"
							  + "\nIf you have 3 emerald orbs, your next hit will increase your movement speed for 30 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Emerald, 8);
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
