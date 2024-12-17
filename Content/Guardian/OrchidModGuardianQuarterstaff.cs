using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.NPC;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianQuarterstaff : OrchidModGuardianParryItem
	{
		public virtual string QuarterstaffTexture => Texture + "_Staff";
		public virtual void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, HitInfo hit, bool jabAttack, bool counterAttack) { } // Called when hitting a target during an attack
		public virtual void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, HitInfo hit, bool jabAttack, bool counterAttack) { } // Called when hitting the first target for the first time during an attack
		public virtual void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit) { } // anchor's modifyhitNPC
		public virtual void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack) { } // Called on the first frame of an attack
		public virtual void OnParryQuarterstaff(Player player, OrchidGuardian guardian, Player.HurtInfo info, Projectile anchor) { } // Called on parrying anything
		public virtual void ExtraAIQuarterstaff(Player player, OrchidGuardian guardian, Projectile projectile) { } // Called at the end of the Anchor Projectile AI
		public virtual void PostDrawQuarterstaff(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { } // Called after the item is done being drawn
		public virtual bool PreDrawQuarterstaff(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; } // Return false to prevent normal draw code
		public virtual Color GetColor(bool offHand) => Color.White;

		public virtual void SafeHoldItem(Player player) { }

		public int ParryDuration = 60; // Parry duration in ticks
		public int SlamStacks; // Stam Stacks given by the item
		public int GuardStacks; // Block Stacks given by the item
		public float JabSpeed = 1f; // Jab speed multiplier
		public float SwingSpeed = 1f; // swing speed multiplier
		public float CounterSpeed = 1f; // spin speed multiplier
		public float JabDamage = 0.5f; // Jab damage multiplier
		public float SwingDamage = 1.5f; // swing damage multiplier
		public float CounterDamage = 1f; // spin damage multiplier
		public float CounterHits = 3; // number of local iframe resets during a counterattack - use scarsely
		public float SwingKnockback = 1.5f; // swing knockback multiplier
		public float JabKnockback = 1f; // spin knockback multiplier
		public float CounterKnockback = 1f; // counter knockback multiplier
		public bool SingleSwing = false; // allows a special swing behaviour

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.DD2_MonkStaffGroundMiss;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTime = 30;
			Item.knockBack = 5f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public sealed override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info, Projectile anchor)
		{
			anchor.ai[2] = -40f;
			(anchor.ModProjectile as GuardianQuarterstaffAnchor).NeedNetUpdate = true;
			OnParryQuarterstaff(player, guardian, info, anchor);
		}

		public override bool WeaponPrefix() => true;

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				var projectileType = ModContent.ProjectileType<GuardianQuarterstaffAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0)
				{

					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianQuarterstaffAnchor anchor)
					{
						bool shouldBlock = Main.mouseRight && Main.mouseRightRelease;
						bool shouldCharge = Main.mouseLeft;

						if (ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs)
						{
							shouldBlock = Main.mouseLeft && Main.mouseLeftRelease;
							shouldCharge = Main.mouseRight;
						}

						if (shouldBlock && !shouldCharge && guardian.UseGuard(1, true) && proj.ai[0] <= 0f && proj.ai[2] == 0f)
						{
							player.immuneTime = 0;
							player.immune = false;
							guardian.modPlayer.PlayerImmunity = 0;
							guardian.GuardianGauntletCharge = 0f;
							guardian.UseGuard(1);
							proj.ai[0] = 0f;
							proj.ai[2] = ParryDuration;
							anchor.NeedNetUpdate = true;
							SoundEngine.PlaySound(SoundID.Item37, player.Center);
						}

						if (shouldCharge && guardian.GuardianGauntletCharge == 0f && proj.ai[0] == 0f && proj.ai[2] >= -10f)
						{
							proj.ai[0] = 1f;
							proj.ai[2] = 0f;
							anchor.NeedNetUpdate = true;
							guardian.GuardianGauntletCharge++;
							SoundEngine.PlaySound(SoundID.Item7, player.Center);
						}
					}
				}
			}
			return false;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianQuarterstaffAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] != 1)
			{
				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == projectileType)
					{
						projectile.Kill();
					}
				}

				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (proj.ModProjectile is not GuardianQuarterstaffAnchor quarterstaff)
				{
					proj.Kill();
				}
				else
				{
					quarterstaff.OnChangeSelectedItem(player);
				}
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianQuarterstaffAnchor quarterstaff)
				{
					if (quarterstaff.SelectedItem != player.selectedItem)
					{
						quarterstaff.OnChangeSelectedItem(player);
					}
				}
			}
			SafeHoldItem(player);
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
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", OrchidUtils.FramesToSeconds((int)(ParryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration())) + " second parry duration"));

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs ? "Left" : "Right";
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", click + " click to parry")
			{
				OverrideColor = new Color(175, 255, 175)
			});

			tooltips.Insert(index + 3, new TooltipLine(Mod, "Swing", "Charge to swing, " + click + " click to jab while charging")
			{
				OverrideColor = new Color(175, 255, 175)
			});

			if (GuardStacks > 0 || SlamStacks > 0)
			{
				string TooltipToGet = ModContent.GetInstance<OrchidMod>().GetLocalizationKey("Misc.GuardianGrants");
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

			/*if (SlamStacks > 0)
			{
				tooltips.Insert(index + 4, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.SlamStacks + " slam" + (this.SlamStacks > 1 ? "s" : "") + " when fully charged")
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}*/
		}
	}
}
