using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Dancer
{
	public abstract class OrchidModDancerItem : OrchidModItem
	{
		public int poiseCost = 0;
		public int poiseChance = 100;
		public int dashTimer = 0;
		public bool vertical = false;
		public bool horizontal = false;
		public float dashVelocity = 1f;
		public OrchidModDancerItemType dancerItemType = OrchidModDancerItemType.NULL;

		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			Item.DamageType = ModContent.GetInstance<DancerDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
		}

		protected override bool CloneNewInstances => true;

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (modPlayer.dancerPoise < this.poiseCost)
			{
				return false;
			}
			else
			{
				Vector2 heading = Main.MouseWorld - player.Center;
				heading.Normalize();
				heading *= new Vector2(this.dashVelocity, this.dashVelocity).Length();
				float speedXAlt = this.horizontal ? heading.X : 0f;
				float speedYAlt = this.vertical ? heading.Y : 0f;

				modPlayer.dancerVelocity = new Vector2(speedXAlt, speedYAlt);
				modPlayer.dancerWeaponDamage = Item.damage;
				modPlayer.dancerWeaponKnockback = Item.knockBack;
				modPlayer.dancerWeaponType = this.dancerItemType;
				modPlayer.dancerDashTimer = this.dashTimer;
				OrchidModDancerHelper.removeDancerPoise(this.poiseChance, this.poiseCost, player, modPlayer, Mod);
			}
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
				tt.Text = damageValue + " dancing " + damageWord;
			}

			string str = null;
			switch (this.dancerItemType)
			{
				case OrchidModDancerItemType.IMPACT:
					str = "-Impact Weapon-";
					break;
				case OrchidModDancerItemType.PHASE:
					str = "-Phase Weapon-";
					break;
				case OrchidModDancerItemType.MOMENTUM:
					str = "-Momentum Weapon-";
					break;
				default:
					break;
			}

			if (str != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", str)
				{
					OverrideColor = new Color(220, 200, 255)
				});
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Dancer Class-")
				{
					OverrideColor = new Color(255, 185, 255)
				});
			}

			if (this.poiseCost > 0)
			{
				tooltips.Insert(tooltips.Count - 1, new TooltipLine(Mod, "PoiseUse", "Uses " + this.poiseCost + " poise"));
			}
		}
	}
}
