using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Dancer
{
	public class DancerDamageClass : DamageClass
	{
		public override bool UseStandardCritCalcs => true;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("dancing damage");
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Generic) return StatInheritanceData.Full;
			return StatInheritanceData.None;
		}

		public override bool GetEffectInheritance(DamageClass damageClass) => false;

		public override void SetDefaultStats(Player player)
		{
		}
	}
}