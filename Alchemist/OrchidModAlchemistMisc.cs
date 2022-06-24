using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistMisc : OrchidModItem
	{
		public virtual void SafeSetDefaults() { }
		public virtual void SafeModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<AlchemistDamageClass>();
			SafeSetDefaults();
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			SafeModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
			/*  [CRIT]
			if (Main.rand.Next(101) <= player.GetModPlayer<OrchidModPlayerAlchemist>().alchemistCrit)
				crit = true;
			else crit = false;
			*/
		}

		protected override bool CloneNewInstances => true;

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
