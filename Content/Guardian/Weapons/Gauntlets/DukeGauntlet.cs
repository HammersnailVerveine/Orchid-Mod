using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class 
	/*Zelda,*/DukeGauntlet/*is under attack by the evil forces of Ganon! I'm going to Gamelon to aid him.*/
	/*But father, what if something happens to you?*/:
	/*I'll take the*/OrchidModGuardianGauntlet/*to protect me. If you don't hear from me in a month, send Link.*/
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
