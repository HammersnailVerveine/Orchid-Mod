using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistEquipable : OrchidModItem
	{
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " chemical " + damageWord;
			}
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}
	}
}
