using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class GuardianHoneyPotionBuff : ModBuff
	{
		public override void Update(Player player, ref int buffIndex)
		{
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianHoneyPotion = true;
		}
	}
}