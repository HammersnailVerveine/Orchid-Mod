using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman
{
	[ClassTag(ClassTags.Shaman)]
	public abstract class OrchidModShamanEquipable : ModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShamanDamageClass>();
			SafeSetDefaults();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " shamanic " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ShamanTag", "-Shaman Class-") // 00C0FF
				{
					OverrideColor = new Color(0, 192, 255)
				});
			}
		}
	}
}
