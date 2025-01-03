using OrchidMod.Content.Shaman.Projectiles.Fire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Fire
{
	public class EnchantedScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<EnchantedScepterProjectile>();
			Element = ShamanElement.FIRE;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
	}
}