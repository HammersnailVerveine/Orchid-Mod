using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianGauntlet : OrchidModGuardianItem
	{
		public virtual string GauntletTexture => Texture + "_Gauntlet";
		public virtual void ExtraAIGauntlet(Projectile projectile) { }
		public virtual void PostAIGauntlet(Projectile projectile) { }
		public virtual void PostDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAIGauntlet(Projectile projectile) { return true; }
		public virtual bool PreDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }

		public virtual void SafeHoldItem(Player player) { }

		public float strikeVelocity = 100f;
		public int parryDuration = 60;

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTime = 30;
			Item.knockBack = 6f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		// public override bool WeaponPrefix() => true;

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
				var projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
				int[] anchors = GetAnchors(player);
				if (anchors != null)
				{
					Projectile projectileMain = Main.projectile[anchors[1]];
					Projectile projectileOff = Main.projectile[anchors[0]];
					if (projectileMain.ai[0] == 0f && guardian.GuardianGauntletCharge == 0)
					{
						if (player.altFunctionUse == 2)
						{ // Right click
							if (guardian.GuardianBlock > 0)
							{
								SoundEngine.PlaySound(SoundID.Item37, player.Center);
								guardian.GuardianBlock--;
								projectileMain.ai[0] = (int)(parryDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration());
								projectileMain.netUpdate = true;
								projectileOff.ai[0] = (int)(parryDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration());
								projectileOff.netUpdate = true;
							}
						}
						else
						{ // Left click
							if (guardian.GuardianGauntletCharge == 0)
							{
								guardian.GuardianGauntletCharge++;
								SoundEngine.PlaySound(SoundID.Item7, player.Center);
							}

							/*
							if (proj.ai[1] + proj.ai[0] == 0f && guardian.GuardianBlock > 0)
							{
								shield.shieldEffectReady = true;
								guardian.GuardianBlock--;
								proj.ai[0] = (int)(blockDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration());
								proj.netUpdate = true;
								proj.netUpdate2 = true;
								BlockStart(player, proj);
							}
							else if (proj.ai[0] > 0f && Main.mouseLeftRelease) // Remove block stance if left click again
							{
								shield.shieldEffectReady = true;
								shield.spawnDusts();
								proj.ai[0] = 0f;
								proj.netUpdate = true;
								proj.netUpdate2 = true;
								resetBlockedEnemiesDuration(guardian);
								BlockStart(player, proj);
							}
							*/
						}
					}
				}
			}
			return false;
		}

		public int[] GetAnchors(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
			int[] anchors = [-1, -1];
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType)
				{
					if (anchors[0] == -1)
					{
						anchors[0] = proj.whoAmI;
					}
					else
					{
						anchors[1] = proj.whoAmI;
						return anchors;
					}
				}
			}

			return null;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;
			int count = player.ownedProjectileCounts[projectileType];
			if (count < 2)
			{
				if (count == 1)
				{
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianGauntletAnchor) proj.Kill();
				}

				int[] indexes = [-1, -1];
				for (int i = 0; i < 2; i++)
				{
					var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

					var proj = Main.projectile[index];
					if (proj.ModProjectile is not GuardianGauntletAnchor gauntlet)
					{
						proj.Kill();
					}
					else
					{
						indexes[i] = proj.whoAmI;
						gauntlet.OffHandGauntlet = i == 0;
						proj.localAI[0] = (int)(parryDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration()); // for UI display
						gauntlet.OnChangeSelectedItem(player);
					}
				}

				if (indexes[1] < indexes[0])
				{ // Swap order if necessary in Main.projectile[] so the front gauntlet is drawn first
					Projectile buffer = Main.projectile[indexes[0]];
					Main.projectile[indexes[0]] = Main.projectile[indexes[1]];
					Main.projectile[indexes[1]] = buffer;
					Main.projectile[indexes[0]].whoAmI = indexes[1];
					Main.projectile[indexes[1]].whoAmI = indexes[0];
				}
			}
			else
			{
				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.type == projectileType && projectile.active && projectile.owner == player.whoAmI && projectile.ModProjectile is GuardianGauntletAnchor gauntlet)
					{
						if (gauntlet.SelectedItem != player.selectedItem)
						{
							projectile.localAI[0] = (int)(parryDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration()); // for UI display
							gauntlet.OnChangeSelectedItem(player);
						}
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

			int tooltipSeconds = Math.DivRem((int)(parryDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration()), 60, out int tooltipTicks);

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", tooltipSeconds + "." + (int)(tooltipTicks * (100 / 60f)) + " parry duration"));

			tooltips.Insert(index + 2, new TooltipLine(Mod, "ShieldStacks", "Right click to parry")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
