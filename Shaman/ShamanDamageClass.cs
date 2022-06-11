using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman
{
	public class ShamanDamageClass : DamageClass
	{
		public override bool UseStandardCritCalcs => true;

		public override void SetStaticDefaults()
		{
			ClassName.SetDefault("shamanic damage");
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Generic) return StatInheritanceData.Full;
			return StatInheritanceData.None;
		}

		public override bool GetEffectInheritance(DamageClass damageClass) => false;

		public override void SetDefaultStats(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 4;
		}
	}
}