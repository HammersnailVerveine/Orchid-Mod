using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class EnchantedRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item176;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 38;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.EnchantedRuneProj>();
			RuneCost = 2;
			RuneDistance = 140f;
			RuneNumber = 2;
			RuneDuration = 20 * 60;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Enchanted Rune");
			// Tooltip.SetDefault("Surrounds you with enchanted sparkles");
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance < 101f ? 101f : distance, GetAmount(guardian));
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance * 0.5f > 95f ? 95f : distance * 0.5f, 2);
		}
	}
}
