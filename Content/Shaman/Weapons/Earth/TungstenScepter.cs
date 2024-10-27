using OrchidMod.Content.Shaman.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace OrchidMod.Content.Shaman.Weapons.Earth
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
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectileTungsten>();
			this.Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Emerald, 8)
			.AddIngredient(ItemID.TungstenBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}