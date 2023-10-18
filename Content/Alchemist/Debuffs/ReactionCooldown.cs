using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Debuffs
{
	public class ReactionCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Experimental drawback");
			// Description.SetDefault("You cannot use alchemist hidden reactions");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}