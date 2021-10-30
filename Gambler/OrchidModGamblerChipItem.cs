using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerChipItem : OrchidModItem
	{
		public int chipCost = 0;
		public int consumeChance = 100;

		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed) {
			return true;
		}

		public sealed override void SetDefaults()
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

		public sealed override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerUIFightDisplay = true;
			modPlayer.gamblerUIChipSpinDisplay = true;
			SafeHoldItem();
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{	
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			mult *= (modPlayer.gamblerDamage + modPlayer.gamblerDamageChip);
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (modPlayer.gamblerChips < this.chipCost || modPlayer.gamblerCardCurrent.type == 0)
			{
				return false;
			}
			else
			{
				OrchidModGamblerHelper.removeGamblerChip(this.consumeChance, this.chipCost, player, modPlayer, mod);
			}
			return base.CanUseItem(player);
		}
		
		public sealed override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerPauseChipRotation = item.useAnimation;
			float speed = new Vector2(speedX, speedY).Length() * -1f;
			return this.SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack, modPlayer, speed);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " gambling " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}

			int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
			if (index != -1)
			{
				if (this.chipCost < 2)
				{
					tooltips.Insert(index, new TooltipLine(mod, "ChipCost", "Uses " + this.chipCost + " chip"));
				}
				else
				{
					tooltips.Insert(index, new TooltipLine(mod, "ChipCost", "Uses " + this.chipCost + " chips"));
				}
			}
		}
	}
}
