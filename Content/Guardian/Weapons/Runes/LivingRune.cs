using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class LivingRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item176;
			Item.useTime = 25;
			Item.knockBack = 5f;
			Item.damage = 21;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.LivingRuneProj>();
			RuneCost = 1;
			RuneDistance = 120f;
			RuneNumber = 2;
			RuneDuration = 20 * 60;
		}
	}
}
