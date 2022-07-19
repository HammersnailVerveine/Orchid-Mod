using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	[ClassTag(ClassTags.Alchemist)]
	public abstract class OrchidModAlchemistEquipable : ModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<AlchemistDamageClass>();
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
				tt.Text = damageValue + " chemical " + damageWord;
			}
		}
	}
}