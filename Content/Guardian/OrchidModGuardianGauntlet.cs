using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.NPC;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianGauntlet : OrchidModGuardianParryItem
	{
		public bool hasArm = false;
		public bool hasShoulder = false;
		public bool hasBackGauntlet = false;

		public virtual string GauntletTexture => Texture + "_Gauntlet";
		public virtual string GauntletBackTexture => Texture + "_GauntletBack";
		public virtual string ArmTexture => Texture + "_Arm";
		public virtual string ShoulderTexture => Texture + "_Shoulder";
		public virtual void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, HitInfo hit, bool charged) { }
		public virtual void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, HitInfo hit, bool charged) { }
		public virtual void ModifyHitNPCGauntlet(Player player, NPC target, Projectile projectile, ref HitModifiers modifiers, bool charged) { }
		public virtual bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage) => true; // Return false to prevent normal punch projectiles from spawning
		public virtual void OnParryGauntlet(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor) { }
		public virtual bool ProjectileAI(Player player, Projectile projectile, bool charged) => true;
		public virtual void ExtraAIGauntlet(Projectile projectile) { }
		public virtual void PostDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }
		public virtual void SafeModifyTooltips(List<TooltipLine> tooltips) { } // Called at the end of ModifyTooltips

		public virtual Color GetColor(bool offHand) => Color.White;

		public virtual void SafeHoldItem(Player player) { }

		public float strikeVelocity = 10f;
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
			Item.knockBack = 5f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public sealed override void OnParry(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			int[] anchors = GetAnchors(player);
			for (int i = 0; i < 2; i++)
			{
				if (Main.projectile[anchors[i]].ai[0] > 0)
				{
					Main.projectile[anchors[i]].ai[0] = 0;
					Main.projectile[anchors[i]].netUpdate = true;
				}
			}
			OnParryGauntlet(player, guardian, aggressor, anchor);
		}

		public override bool WeaponPrefix() => true;

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

					bool shouldBlock = Main.mouseRight && Main.mouseRightRelease;
					bool shoundpunch = Main.mouseLeft && Main.mouseLeftRelease;
					if (ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs)
					{
						shouldBlock = Main.mouseLeft && Main.mouseLeftRelease;
						shoundpunch = Main.mouseRight && Main.mouseRightRelease;
					}

					if (projectileMain.ai[0] == 0f || projectileOff.ai[0] == 0f || (projectileMain.ai[0] > 0f && projectileOff.ai[0] > 0f))
					{ // At least one of the gauntlets is not being used or both are blocking
						if (shouldBlock)
						{ // Right click & None of the gauntlets is blocking = Block
							if (guardian.UseGuard(1, true) && (projectileMain.ai[0] <= 0f || projectileOff.ai[0] <= 0f) 
								&& !(projectileMain.ai[0] > 0 && projectileMain.ai[2] <= 0) && !(projectileOff.ai[0] > 0 && projectileOff.ai[2] <= 0))
							{ // Player is not already blocking with a gauntlet
								player.immuneTime = 0;
								guardian.modPlayer.PlayerImmunity = 0;
								player.immune = false;
								SoundEngine.PlaySound(SoundID.Item37, player.Center);
								guardian.UseGuard(1);
								if (projectileMain.ai[0] == 0)
								{
									projectileMain.ai[0] = (int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration);
									(projectileMain.ModProjectile as GuardianGauntletAnchor).NeedNetUpdate = true;
								}

								if (projectileOff.ai[0] == 0)
								{
									projectileOff.ai[0] = (int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration);
									(projectileOff.ModProjectile as GuardianGauntletAnchor).NeedNetUpdate = true;
								}
							}
						}
						else if (shoundpunch)
						{ // Left click
							if (guardian.GuardianGauntletCharge == 0)
							{
								//guardian.GuardianGauntletCharge++;
								SoundEngine.PlaySound(SoundID.Item7, player.Center);

								if (projectileMain.ai[0] != 0f)
								{ // Main gauntlet is slamming or blocking, use offhand one
									projectileOff.ai[2] = 1f;
									(projectileOff.ModProjectile as GuardianGauntletAnchor).NeedNetUpdate = true;
								}
								else
								{ // else use main hand
									projectileMain.ai[2] = 1f; 
									(projectileMain.ModProjectile as GuardianGauntletAnchor).NeedNetUpdate = true;
								}
							}
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
						proj.localAI[0] = (int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration); // for UI display
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
							projectile.localAI[0] = (int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration); // for UI display
							gauntlet.OnChangeSelectedItem(player);
						}
					}
				}
			}
			SafeHoldItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var guardian = Main.LocalPlayer.GetModPlayer<OrchidGuardian>();
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", OrchidUtils.FramesToSeconds((int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration)) + " second parry duration"));

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs ? "Left" : "Right";
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", click + " click to parry")
			{
				OverrideColor = new Color(175, 255, 175)
			});

			SafeModifyTooltips(tooltips);
		}

		public virtual Texture2D GetGauntletTexture(bool OffHandGauntlet, out Rectangle? drawRectangle)
		{
			drawRectangle = null;
			if (hasBackGauntlet && OffHandGauntlet)
			{
				return(ModContent.Request<Texture2D>(GauntletBackTexture).Value);
			}
			else
			{
				return (ModContent.Request<Texture2D>(GauntletTexture).Value);
			}
		}

		public virtual Texture2D GetArmTexture(out Rectangle? drawRectangle)
		{
			drawRectangle = null;
			return ModContent.Request<Texture2D>(ArmTexture).Value;
		}

		public virtual Texture2D GetShoulderTexture(out Rectangle? drawRectangle)
		{
			drawRectangle = null;
			return ModContent.Request<Texture2D>(ShoulderTexture).Value;
		}
	}
}
