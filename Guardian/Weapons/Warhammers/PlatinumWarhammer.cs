using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Warhammers
{
	public class PlatinumWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 0, 26, 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.knockBack = 8f;
			Item.shootSpeed = 9f;
			Item.damage = 53;
			this.range = 35;
			this.blockStacks = 1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Platinum Warhammer");
			// Tooltip.SetDefault("Hurls a heavy hammer");
		}

		public override bool ThrowAI(Player player, OrchidGuardian guardian, bool weak)
		{
			return true;
		}

		public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak)
		{
		}

		public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak)
		{
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.PlatinumBar, 8);
			recipe.Register();
		}
	}
}
