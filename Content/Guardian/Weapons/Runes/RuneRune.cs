using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class RuneRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 2, 90);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item176;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 112;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.RuneRuneProj>();
			RuneCost = 3;
			RuneNumber = 1;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{

			for (int i = 0; i < GetNumber(guardian); i++)
			{
				float angle = 90f - (GetNumber(guardian) - 1) * 10f + 20 * i;
				NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, 2, angle + 90f, 1f);
				NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, 2, angle);
			}
		}
	}
}
