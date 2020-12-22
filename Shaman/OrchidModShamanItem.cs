using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanItem : ModItem
	{
		public int empowermentType = 0;
		public int empowermentLevel = 0;
		
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			Item.staff[item.type] = true;
			item.crit = 4;
			item.useStyle = 5;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().shamanCrit;
		}
		
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).shamanCrit)
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
				tt.text = damageValue + " shamanic " + damageWord;
			}
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
				if (index != -1)
				{
					tooltips.Insert(index + 1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
			
			if (this.empowermentType > 0) {
				string emp = "";
				Color col = new Color(0, 0, 0);
				switch (this.empowermentType) {
					case 1:
						emp = "Fire";
						col = new Color(194, 38, 31);
						break;
					case 2:
						emp = "Water";
						col = new Color(0, 119, 190);
						break;
					case 3:
						emp = "Air";
						col = new Color(75, 139, 59);
						break;
					case 4:
						emp = "Earth";
						col = new Color(255, 255, 102);
						break;
					case 5:
						emp = "Spirit";
						col = new Color(138, 43, 226);
						break;
					default:
						break;
				}
				
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					tooltips.Insert(index, new TooltipLine(mod, "BondType", emp + " bond")
					{
						overrideColor = col
					});
				}
			}
			
			if (this.empowermentLevel > 0) {
				string lev = "";
				switch (this.empowermentLevel) {
					case 1:
						lev = "I";
						break;
					case 2:
						lev = "II";
						break;
					case 3:
						lev = "III";
						break;
					case 4:
						lev = "IV";
						break;
					case 5:
						lev = "V";
						break;
					default:
						break;
				}
				
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					tooltips.Insert(index, new TooltipLine(mod, "BondLevel", "Shamanic bond level " + lev)
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
		}
	}
}
