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
			Item.value = Item.sellPrice(0, 0, 0, 15);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 25;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.EnchantedRuneProj>();
			this.cost = 2;
			this.duration = 3600;
			this.distance = 140f;
			this.number = 2;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Enchanted Rune");
			// Tooltip.SetDefault("Surrounds you with enchanted sparkles");
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance < 101f ? 101f : distance, number);
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance * 0.5f > 95f ? 95f : distance * 0.5f, number);
		}
	}
}
