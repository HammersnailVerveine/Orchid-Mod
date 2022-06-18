using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerEquipable : OrchidModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " gambling " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Gambler Class-")
				{
					OverrideColor = new Color(255, 200, 0)
				});
			}
		}
	}
}
