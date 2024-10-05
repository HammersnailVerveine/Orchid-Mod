using Microsoft.Xna.Framework;
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
			Item.value = Item.sellPrice(0, 8, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item176;
			Item.useTime = 20;
			Item.knockBack = 3f;
			Item.damage = 95;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.FrostRuneProj>();
			RuneCost = 3;
			RuneNumber = 12;
			RuneAmountScaling = 3;
			RuneDuration = 40 * 60;
		}

		public override Color GetGlowColor() => new Color(255, 0, 0);

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int amount)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, amount);
		}
	}
}
