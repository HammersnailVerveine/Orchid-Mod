using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Buffs
{
	public class DemonBreathFlaskBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Demon Reek");
			// Description.SetDefault("Demon breath will create more projectiles");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}