using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class PirateWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 3, 40, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 10f;
			Item.shootSpeed = 12f;
			Item.damage = 220;
			Item.useTime = 25;
			range = 40;
			blockStacks = 2;
			slamStacks = 1;
		}

		public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Midas, 300);
		}
	}
}
