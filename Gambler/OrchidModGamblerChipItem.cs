using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerChipItem : OrchidModItem
	{
		public int chipCost = 0;
		public int consumeChance = 100;
		public bool pauseRotation = true;

		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem(Player player, OrchidModPlayerGambler modPlayer) { }
		public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack, OrchidModPlayerGambler modPlayer, float speed) {
			return true;
		}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			Item.DamageType = ModContent.GetInstance<GamblerChipDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
		}

		protected override bool CloneNewInstances => true;

		public sealed override void HoldItem(Player player)
		{
			OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();
			modPlayer.gamblerUIFightDisplay = true;
			modPlayer.gamblerUIChipSpinDisplay = true;
			SafeHoldItem(player, modPlayer);
		}

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();

			if (modPlayer.gamblerChips < this.chipCost || modPlayer.gamblerCardCurrent.type == ItemID.None)
			{
				return false;
			}
			else
			{
				modPlayer.RemoveGamblerChip(this.consumeChance, this.chipCost);
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();
			modPlayer.gamblerPauseChipRotation = (pauseRotation ? Item.useAnimation : modPlayer.gamblerPauseChipRotation);
			float speed = velocity.Length() * -1f;
			return this.SafeShoot(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, modPlayer, speed);
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
