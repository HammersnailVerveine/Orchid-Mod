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
		public bool pauseRotation = true;

		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem(Player player, OrchidModPlayer modPlayer) { }
		public virtual void SafeModifyWeaponDamage(Player player, OrchidModPlayer modPlayer, ref float add, ref float mult, ref float flat) { }
		public virtual bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed) {
			return true;
		}

		public sealed override void SetDefaults()
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

		public sealed override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerUIFightDisplay = true;
			modPlayer.gamblerUIChipSpinDisplay = true;
			SafeHoldItem(player, modPlayer);
		}

		public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{	
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			mult *= (modPlayer.gamblerDamage + modPlayer.gamblerDamageChip);
			SafeModifyWeaponDamage(player, modPlayer, ref add, ref mult, ref flat);
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
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
				OrchidModGamblerHelper.removeGamblerChip(this.consumeChance, this.chipCost, player, modPlayer, Mod);
			}
			return base.CanUseItem(player);
		}
		
		public sealed override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerPauseChipRotation = (pauseRotation ? Item.useAnimation : modPlayer.gamblerPauseChipRotation);
			float speed = new Vector2(speedX, speedY).Length() * -1f;
			return this.SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack, modPlayer, speed);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " gambling " + damageWord;
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Gambler Class-")
				{
					OverrideColor = new Color(255, 200, 0)
				});
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
			if (index != -1)
			{
				if (this.chipCost < 2)
				{
					tooltips.Insert(index, new TooltipLine(Mod, "ChipCost", "Uses " + this.chipCost + " chip"));
				}
				else
				{
					tooltips.Insert(index, new TooltipLine(Mod, "ChipCost", "Uses " + this.chipCost + " chips"));
				}
			}
		}
	}
}
