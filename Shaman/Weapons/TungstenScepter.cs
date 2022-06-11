using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class TungstenScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 24;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 58;
			Item.useAnimation = 58;
			Item.knockBack = 4.25f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("TungstenScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an emerald orb"
							  + "\nIf you have 3 emerald orbs, your next hit will increase your movement speed for 30 seconds");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Emerald, 8);
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
