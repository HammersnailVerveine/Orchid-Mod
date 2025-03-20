using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Dancer
{
	[ClassTag(ClassTags.Dancer)]
	public abstract class OrchidModDancerItem : ModItem
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
			OrchidDancer modPlayer = player.GetModPlayer<OrchidDancer>();

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
				modPlayer.RemoveDancerPoise(this.poiseChance, this.poiseCost);
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
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.DancerDamageClass.DisplayName"));
			}

			string str = null;
			switch (this.dancerItemType)
			{
				case OrchidModDancerItemType.IMPACT:
					str = Language.GetTextValue("Mods.OrchidMod.UI.DanserItem.ImpactWeapon");
					break;
				case OrchidModDancerItemType.PHASE:
					str = Language.GetTextValue("Mods.OrchidMod.UI.DanserItem.PhaseWeapon");
					break;
				case OrchidModDancerItemType.MOMENTUM:
					str = Language.GetTextValue("Mods.OrchidMod.UI.DanserItem.MomentumWeapon");
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

			if (this.poiseCost > 0)
			{
				tooltips.Insert(tooltips.Count - 1, new TooltipLine(Mod, "PoiseUse", "Uses " + this.poiseCost + " poise"));
			}
		}
	}
}
