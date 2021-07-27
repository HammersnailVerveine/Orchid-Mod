using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanEquipable : OrchidModItem
	{
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().shamanCrit;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " shamanic " + damageWord;
			}
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
				{
					overrideColor = new Color(0, 192, 255)
				});
			}
		}
	}
}
