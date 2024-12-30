using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianHammer : OrchidModGuardianItem
	{
		public int Range;
		public int SlamStacks;
		public int GuardStacks;
		public bool Penetrate;
		public bool TileCollide;
		public float ReturnSpeed;
		public virtual void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged) { } // Called upon landing any melee swing hit
		public virtual void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged) { } // Called upon landing the first hit of a melee swing
		public virtual void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak) { } // Called upon landing any throw hit
		public virtual void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak) { } // Called upon landing the first hit of a throw
		public virtual void OnSwing(Player player, OrchidGuardian guardian, Projectile projectile, bool FullyCharged) { } // Called on the first frame of a throw, FullyCharged is true if the guardian's hammer charge is full
		public virtual void OnThrow(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak) { } // Called on the first frame of a swing
		public virtual void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile) { } // Called at the end of the anchors Projectile AI()
		/// <summary>Called before default throw AI. Return false to prevent the default AI from running.</summary>
		/// <remarks>Remember to set <c>Projectile.friendly</c> and <c>OrchidModGuardianProjectile.ResetHitStatus()</c> if overriding default behavior.</remarks>
		public virtual bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak) => true;

		public sealed override void SetDefaults()
		{
			Item.DamageType = GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.knockBack = 10f;
			Item.shootSpeed = 10f;
			Range = 0;
			Penetrate = false;
			TileCollide = true;
			SlamStacks = 0;
			ReturnSpeed = 1f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();

			Item.useAnimation = Item.useTime;
		}

		public override bool WeaponPrefix() => true;

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;
		}

		public override bool? UseItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			int projType = ProjectileType<GuardianHammerAnchor>();

			int damage = guardian.GetGuardianDamage(Item.damage);
			Projectile projectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.Zero, projType, damage, Item.knockBack, player.whoAmI);
			projectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);

			guardian.GuardianHammerCharge = 0f;
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			int projType = ProjectileType<GuardianHammerAnchor>();
			if (player.ownedProjectileCounts[projType] > 0) return false;
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			if (GuardStacks > 0 || SlamStacks > 0)
			{
				string TooltipToGet = GetInstance<OrchidMod>().GetLocalizationKey("Misc.GuardianGrants");
				switch(GuardStacks)
				{
					case 1: TooltipToGet += "Guard"; break;
					case >1: TooltipToGet += "Guards"; break;
				}
				switch (SlamStacks)
				{
					case 1: TooltipToGet += "Slam"; break;
					case >1: TooltipToGet += "Slams"; break;
				}
				if (GuardStacks == SlamStacks) TooltipToGet += "Same";

				tooltips.Insert(index + 1, new TooltipLine(Mod, "GuardianGrants", Language.GetText(TooltipToGet).Format(GuardStacks, SlamStacks))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

				/*tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + (GuardStacks > 0 ? GuardStacks + " guard" + (GuardStacks > 1 ? "s" : "") + (SlamStacks > 0 ? " and " : "") : "") + (SlamStacks > 0 ? (GuardStacks != SlamStacks ? SlamStacks + " " : "") + "slam" + (SlamStacks > 1 ? "s" : "") : "") + " when fully charged")
				{
					OverrideColor = new Color(175, 255, 175)
				});*/

			/*if (SlamStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.SlamStacks + " slam" + (this.SlamStacks > 1 ? "s" : "") + " when fully charged")
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}*/

			tooltips.Insert(index + 1, new TooltipLine(Mod, "Swing", "Charge to throw, right click to swing while charging")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
