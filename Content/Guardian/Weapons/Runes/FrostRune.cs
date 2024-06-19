using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class FrostRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 8, 50);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item176;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 95;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.FrostRuneProj>();
			RuneCost = 3;
			RuneNumber = 4;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, GetNumber(guardian) * 3);
		}
	}
}
