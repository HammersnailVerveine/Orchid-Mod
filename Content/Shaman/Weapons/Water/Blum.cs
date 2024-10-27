using OrchidMod.Content.Shaman.Projectiles.Water;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Water
{
	public class Blum : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 17;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 47, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<BlumProjectile>();
			this.Element = ShamanElement.WATER;
			this.CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			Item.useTime = 18;
			if (modPlayer.CountShamanicBonds() > 1) Item.useTime = (int)(Item.useTime * 0.51f);
		}
	}
}