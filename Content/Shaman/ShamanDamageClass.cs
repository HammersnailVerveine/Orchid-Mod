using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman
{
	public class ShamanDamageClass : DamageClass
	{
		public override bool UseStandardCritCalcs => true;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Generic) return StatInheritanceData.Full;
			return StatInheritanceData.None;
		}

		public override bool GetEffectInheritance(DamageClass damageClass) => false;

		public override bool ShowStatTooltipLine(Player player, string lineName)
		{
			if (lineName == "Speed") return false;
            else return true;
		}

		/*public override void SetDefaultStats(Player player)
		{
		}*/
	}
}