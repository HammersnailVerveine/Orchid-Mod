using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianShield : OrchidModGuardianItem
	{
		public virtual string ShieldTexture => "OrchidMod/Content/Guardian/ShieldTextures/" + this.Name + "_Shield";
		public virtual void ExtraAIShield(Projectile projectile) { }
		public virtual void PostAIShield(Projectile projectile) { }
		public virtual void PostDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAIShield(Projectile projectile) { return true; }
		public virtual bool PreDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }

		public virtual void SafeHoldItem(Player player) { }
		public virtual void SlamHitFirst(Player player, Projectile shield, NPC npc) { } // Called up to once upon hitting an enemy when slamming
		public virtual void SlamHit(Player player, Projectile shield, NPC npc) { } // Called upon hitting an enemy when slamming
		public virtual void Slam(Player player, Projectile shield) { } // Called upon slamming

		public virtual void Push(Player player, Projectile shield, NPC npc) { } // Called upon pushing an enemy while blocking
		public virtual void Protect(Player player, Projectile shield) { } // Called up to once when the first projectile is blocked or enemy pushed
		public virtual void Block(Player player, Projectile shield, Projectile projectile) {  // Called when any projectile is blocked
			projectile.Kill();
		}
		public virtual void BlockStart(Player player, Projectile shield) { }  // Called when the player starts blocking (as they press the left click)



		public float distance = 100f;
		public float bashDistance = 100f;
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

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{			
			if (player.whoAmI == Main.myPlayer)
			{
				var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0) {
					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
					{
						if (player.altFunctionUse == 2) { // Right click
							if (proj.ai[1] == 0f && guardian.GuardianSlam > 0) 
							{
								SoundEngine.PlaySound(Item.UseSound, player.Center);
								guardian.GuardianSlam --;
								shield.shieldEffectReady = true;
								proj.ai[1] = Item.useTime;
								if (proj.ai[0] > 0f) 
								{
									shield.spawnDusts();
									proj.ai[0] = 0f;
									resetBlockedEnemiesDuration(guardian);
								}
								proj.netUpdate = true;
								proj.netUpdate2 = true;
							}
						} else { // Left click
							if (proj.ai[1] + proj.ai[0] == 0f && guardian.GuardianBlock > 0) 
							{
								shield.shieldEffectReady = true;
								guardian.GuardianBlock --;
								proj.ai[0] = blockDuration;
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
						}
					}
				}
			}
			return false;
		}

		public GuardianShieldAnchor GetAnchor(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
				{
					return shield;
				}
			}
			return null;
		}

		public void resetBlockedEnemiesDuration(OrchidGuardian modPlayer) {
			for (int i = modPlayer.GuardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = modPlayer.GuardianBlockedEnemies[i];
				blockedEnemy.time = blockedEnemy.time < 60 ? blockedEnemy.time : 60;
			}
		}
		
		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (!(proj.ModProjectile is GuardianShieldAnchor shield))
				{
					proj.Kill();
				}
				else 
				{
					proj.localAI[0] = this.blockDuration;
					shield.OnChangeSelectedItem(player);
				}
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
				{
					if (shield.SelectedItem != player.selectedItem)
					{
						proj.localAI[0] = this.blockDuration;
						shield.OnChangeSelectedItem(player);
					}
				}
			}
			this.SafeHoldItem(player);
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
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Right click to slam")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
