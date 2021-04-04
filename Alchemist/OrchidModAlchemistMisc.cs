using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistMisc : OrchidModItem
	{
		public virtual void SafeSetDefaults() {}
		public virtual void SafeModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {}
		
		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}
		
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			SafeModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
			if (Main.rand.Next(101) <= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).alchemistCrit)
                crit = true;
			else crit = false;
		}
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " chemical " + damageWord;
			}
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}
	}
}
