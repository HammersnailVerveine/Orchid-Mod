using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Shields
{
	public class WoodenPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.knockBack = 5f;
			Item.damage = 10;
			Item.useTime = 35;
			this.distance = 35f;
			this.bashDistance = 70f;
			this.blockDuration = 60;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Pavise");
			Tooltip.SetDefault("owo");
		}
		
		public override void SlamHitFirst(Player player, Projectile shield, NPC npc) { 
			//player.HealEffect(1, true);
		}

		// public override void AddRecipes()
		// {
		// ModRecipe recipe = new ModRecipe(mod);
		// recipe.AddTile(TileID.WorkBenches);		
		// recipe.AddIngredient(ItemID.Wood, 8);
		// recipe.SetResult(this);
		// recipe.AddRecipe();
		// }
	}
}
