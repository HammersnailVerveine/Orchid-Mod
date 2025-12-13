using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	[ClassTag(ClassTags.Guardian)]
	public abstract class OrchidModGuardianEquipable : ModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
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
				tt.Text = $"{damageValue} {Language.GetTextValue("Mods.OrchidMod.DamageClasses.GuardianDamageClass.DisplayName")}";
			}
		}
	}
}