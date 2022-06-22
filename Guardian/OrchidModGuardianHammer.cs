using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Guardian;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianHammer : OrchidModGuardianItem
	{
		public virtual void Hit(Player player, NPC target) { } // Called upon hitting an enemy with any hit
		public virtual void HitStrong(Player player, NPC target) { } // Called upon hitting an enemy with a strong or max hit
		public virtual void HitMax(Player player, NPC target) { } // Called upon hitting an enemy with a max hit

		public Color attackColor = Color.White; // The displayed hammer slash color

		public sealed override void SetDefaults()
		{
			Item.crit = 4;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 6f;
			Item.DamageType = DamageClass.Generic;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
		}
		
		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */ {
			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{			
			if (player == Main.LocalPlayer)
			{
				Vector2 dir = Main.MouseWorld - player.Center;
				dir.Normalize();
				if (player.altFunctionUse == 2 && Main.mouseRightRelease) { // Right click
					dir *= 50f;
					int projType = ProjectileType<Guardian.HammerSlashStrong>();
					Projectile projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center.X + dir.X, player.Center.Y + dir.Y, dir.X * 0.0001f, dir.Y * 0.0001f, projType, 10, 10f, player.whoAmI)];
					projectile.rotation = dir.ToRotation();
					projectile.direction = projectile.spriteDirection;
					projectile.netUpdate = true;
				} else if (Main.mouseLeft && Main.mouseLeftRelease) { // Left click
					dir *= 40f;
					int projType = ProjectileType<Guardian.HammerSlash>();
					Projectile projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center.X + dir.X, player.Center.Y + dir.Y, dir.X * 0.0001f, dir.Y * 0.0001f, projType, 10, 5f, player.whoAmI)];
					projectile.rotation = dir.ToRotation();
					projectile.direction = projectile.spriteDirection;
					projectile.netUpdate = true;
				}
			}
			return false;
		}
	}
}
