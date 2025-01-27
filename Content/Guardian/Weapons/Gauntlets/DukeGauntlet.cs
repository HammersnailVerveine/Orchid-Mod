using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class DukeGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 40;
			Item.knockBack = 3f;
			Item.damage = 53;
			Item.value = Item.sellPrice(0, 0, 8, 40);
			Item.rare = ItemRarityID.White;
			Item.useTime = 35;
			strikeVelocity = 15f;
			parryDuration = 60;
			hasBackGauntlet = true;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(32, 150, 178);
		}
	}
}
