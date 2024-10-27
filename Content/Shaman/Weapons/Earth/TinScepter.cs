using OrchidMod.Content.Shaman.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Earth
{
	public class TinScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 16;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 6, 0);
			Item.UseSound = SoundID.Item45;
			Item.shootSpeed = 6.5f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectileTin>();
			this.Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Topaz, 8)
			.AddIngredient(ItemID.TinBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}