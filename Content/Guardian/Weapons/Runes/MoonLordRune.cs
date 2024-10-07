using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class MoonLordRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item117;
			Item.useTime = 35;
			Item.knockBack = 5f;
			Item.damage = 362;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.MoonLordRuneProj>();
			RuneCost = 5;
			RuneNumber = 3;
			RuneAmountScaling = 1;
			RuneDuration = 60 * 60;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int amount)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, amount);
		}
	}
}
