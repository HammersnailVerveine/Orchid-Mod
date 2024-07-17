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

		public float slamDistance = 100f;
		public int blockDuration = 60;

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
			/*
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0) {
					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
					{

					}
				}
			}
			*/
			return false;
		}

		public GuardianGauntletAnchor GetAnchor(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType && i.ai[0] == 0);
				if (proj != null && proj.ModProjectile is GuardianGauntletAnchor gauntlet)
				{
					return gauntlet;
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

				for (int i = 0; i < 2; i ++)
				{
					var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

					var proj = Main.projectile[index];
					if (proj.ModProjectile is not GuardianGauntletAnchor gauntlet)
					{
						proj.Kill();
					}
					else
					{
						proj.ai[1] = i;
						proj.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration());
						gauntlet.OnChangeSelectedItem(player);
					}
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
							projectile.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration());
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

			int tooltipSeconds = Math.DivRem((int)(blockDuration * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetBlockDuration()), 60, out int tooltipTicks);

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "BlockDuration", tooltipSeconds + "." + (int)(tooltipTicks * (100 / 60f)) + " block duration"));

			tooltips.Insert(index + 2, new TooltipLine(Mod, "ShieldStacks", "Right click to guard")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
