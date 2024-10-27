using OrchidMod.Content.Shaman.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Earth
{
	public class AmberScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 26;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.75f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectileAmber>();
			Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer)
		{
			shamanPlayer.ShamanEarthBond += 300;
			shamanPlayer.modPlayer.TryHeal(20);
		}

		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Amber, 8)
			.AddIngredient(ItemID.FossilOre, 15)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}