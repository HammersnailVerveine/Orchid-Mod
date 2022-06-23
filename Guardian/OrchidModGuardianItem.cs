using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianItem : OrchidModItem
	{
		public virtual void SafeSetDefaults() { }

		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
			SafeSetDefaults();
		}

		protected override bool CloneNewInstances => true;

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage *= player.GetModPlayer<OrchidModPlayer>().guardianDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().guardianCrit;
		}

		public override bool CanUseItem(Player player)
		{
			//OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " opposing " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Guardian Class-")
				{
					OverrideColor = new Color(165, 130, 100)
				});
			}
		}
	}
}
