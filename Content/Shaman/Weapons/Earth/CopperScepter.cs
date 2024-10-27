using OrchidMod.Content.Shaman.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Earth
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
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 4, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 6f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectileCopper>();
			this.Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Amethyst, 8)
			.AddIngredient(ItemID.CopperBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}