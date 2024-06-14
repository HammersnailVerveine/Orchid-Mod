using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class GuardianSpikeBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage<GuardianDamageClass>() += 0.15f;
		}
	}
}