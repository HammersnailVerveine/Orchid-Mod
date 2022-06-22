using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Guardian;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianShield : OrchidModGuardianItem
	{
		public virtual string ShieldTexture => "OrchidMod/Guardian/ShieldTextures/" + this.Name + "_Shield";
		public virtual void ExtraAIShield(Projectile projectile, bool after) { }
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
		
		public float distance = 100f;
		public float bashDistance = 100f;
		public int blockDuration = 60;

		public sealed override void SetDefaults()
		{
			Item.crit = 4;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTime = 30;
			Item.knockBack = 6f;
			Item.DamageType = DamageClass.Generic;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
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
			if (player == Main.LocalPlayer)
			{
				var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0) {
					var guardian = player.GetModPlayer<OrchidModPlayer>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
					{
						if (player.altFunctionUse == 2) { // Right click
							if (proj.ai[1] == 0f && guardian.guardianSlam > 0) 
							{
								guardian.guardianSlam --;
								shield.shieldEffectReady = true;
								proj.ai[1] = this.bashDistance;
								if (proj.ai[0] > 0f) 
								{
									shield.spawnDusts();
									proj.ai[0] = 0f;
									resetBlockedEnemiesDuration(guardian);
								}
								proj.netUpdate = true;
							}
						} else { // Left click
							if (proj.ai[1] + proj.ai[0] == 0f && guardian.guardianBlock > 0) 
							{
								shield.shieldEffectReady = true;
								guardian.guardianBlock --;
								proj.ai[0] = this.blockDuration;
								proj.netUpdate = true;
							}
							else if (proj.ai[0] > 0f && Main.mouseLeftRelease) // Remove block stance if left click again
							{
								shield.shieldEffectReady = true;
								shield.spawnDusts();
								proj.ai[0] = 0f;
								proj.netUpdate = true;
								resetBlockedEnemiesDuration(guardian);
							} 
						}
					}
				}
			}
			return false;
		}
		
		public void resetBlockedEnemiesDuration(OrchidModPlayer modPlayer) {
			for (int i = modPlayer.guardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = modPlayer.guardianBlockedEnemies[i];
				blockedEnemy.time = blockedEnemy.time < 60 ? blockedEnemy.time : 60;
			}
		}
		
		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			var guardian = player.GetModPlayer<OrchidModPlayer>();
			guardian.guardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

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

	}
}
