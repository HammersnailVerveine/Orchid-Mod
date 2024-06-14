using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class HellRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 1, 75);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item176;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 67;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.HellRuneProj>();
			RuneCost = 2;
			RuneNumber = 1;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			for (int i = 0; i < GetNumber(guardian); i ++)
			{
				float angle = 90f - (GetNumber(guardian) - 1) * 10f + 20 * i;
				NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, 2, angle);
			}
		}
	}
}
