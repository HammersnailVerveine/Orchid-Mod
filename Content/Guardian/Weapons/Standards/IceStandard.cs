using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class IceStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 35, 75);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 35;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			blockStacks = 1;
			flagOffset = 8;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(106, 210, 255);
		}
	}
}
