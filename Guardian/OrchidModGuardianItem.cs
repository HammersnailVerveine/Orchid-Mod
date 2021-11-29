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
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
		}

		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().guardianDamage;
		}

		public override void GetWeaponCrit(Player player, ref int crit)
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
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " opposing " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Guardian Class-")
				{
					overrideColor = new Color(165, 130, 100)
				});
			}
		}
	}
}
