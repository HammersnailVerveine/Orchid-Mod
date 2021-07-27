using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerEquipable : OrchidModItem
	{
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " gambling " + damageWord;
			}
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}
		}
	}
}
