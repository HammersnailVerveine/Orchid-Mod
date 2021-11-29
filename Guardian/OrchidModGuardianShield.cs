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
		public virtual void Block(Player player, Projectile shield, Projectile projectile) { 
			projectile.Kill();
		}
		
		public float distance = 25f;

		public sealed override void SetDefaults()
		{
			item.crit = 4;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
			item.useStyle = ItemUseStyleID.Stabbing;

			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;
			
			this.distance = 10f;

			this.SafeSetDefaults();
		}
		
		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetOrchidPlayer();
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);
				guardian.guardianShieldIndex = index;

				var proj = Main.projectile[index];
				if (!(proj.modProjectile is GuardianShieldAnchor shield))
				{
					proj.Kill();
					guardian.guardianShieldIndex = -1;
				}
				else shield.OnChangeSelectedItem(player);
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.modProjectile is GuardianShieldAnchor shield)
				{
					if (shield.SelectedItem != player.selectedItem)
					{
						shield.OnChangeSelectedItem(player);
					}
				}
				else guardian.guardianShieldIndex = -1;
			}
			this.SafeHoldItem(player);
		}

	}
}
