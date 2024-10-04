using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class BeeRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item176;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 22;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.BeeRuneProj>();
			RuneCost = 2;
			RuneNumber = 3;
			RuneDuration = 35 * 60;
		}
	}
}
