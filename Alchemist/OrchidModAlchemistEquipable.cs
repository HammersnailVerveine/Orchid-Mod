using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistEquipable : OrchidModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			Item.DamageType = DamageClass.Generic;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " chemical " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Alchemist Class-")
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}
		}
	}
}
