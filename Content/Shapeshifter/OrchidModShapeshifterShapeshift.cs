using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	public abstract class OrchidModShapeshifterShapeshift : OrchidModShapeshifterItem
	{
		public int ShapeshiftWidth;
		public int ShapeshiftHeight;
		public bool AutoReuseLeft;
		public bool AutoReuseRight;

		public virtual string ShapeshiftTexture => Texture + "_Shapeshift";
		public virtual void PostDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { } // Called after drawing the shapeshift anchor
		public virtual bool PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; } // Called before drawing the shapeshift anchor, return false to prevent it
		public virtual void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) { } // Very important, this is the AI of the shapeshift
		public virtual void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) { } // Makes stuff happen when the player just transformed
		public virtual void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor) { } // Makes stuff happen when the anchor disappears (player leaves shapeshift form)

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShapeshifterDamageClass>();
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 0f;
			ShapeshiftWidth = 10;
			ShapeshiftHeight = 10;
			AutoReuseLeft = true;
			AutoReuseRight = false;

			SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		public override bool CanUseItem(Player player)
		{
			int projectileType = ModContent.ProjectileType<ShapeshifterShapeshiftAnchor>();
			if (player.ownedProjectileCounts[projectileType] != 1)
			{
				if (IsLocalPlayer(player))
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
					if (proj.ModProjectile is not ShapeshifterShapeshiftAnchor anchor)
					{
						proj.Kill();
					}
					else
					{
						anchor.OnChangeSelectedItem(player);
						anchor.NeedNetUpdate = true;
					}
				}
			}
			return false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
