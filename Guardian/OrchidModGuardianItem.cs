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
			Item.melee = false;
			Item.ranged = false;
			Item.magic = false;
			Item.thrown = false;
			Item.summon = false;
			Item.noMelee = true;
			Item.maxStack = 1;
		}

		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().guardianDamage;
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
