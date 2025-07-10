using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs.Debuffs
{
	public class ShapeshifterShapeshiftingCooldownDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}