using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class GoldScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 28;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.75f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = Mod.Find<ModProjectile>("GoldScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you a ruby orb"
							  + "\nIf you have 3 ruby orbs, your next hit will increase your life regeneration for 30 seconds");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
