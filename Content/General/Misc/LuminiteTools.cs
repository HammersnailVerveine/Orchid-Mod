using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Assets;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Misc
{
	public abstract class LuminiteTool : ModItem
	{
		private readonly Color lightColor;
		private readonly int itemCloneType;

		// ...

		public LuminiteTool(Color lightColor, int itemCloneType)
		{
			this.lightColor = lightColor;
			this.itemCloneType = itemCloneType;
		}

		// ...

		public virtual void SafeSetDefaults() { }
		public virtual int GetProjectileType()
			=> ProjectileID.None;

		public sealed override void SetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, OrchidUtils.DrawSimpleItemGlowmaskOnPlayer);

			// DisplayName.SetDefault(name);
		}

		public sealed override void SetDefaults()
		{
			Item.CloneDefaults(itemCloneType);
			Item.glowMask = -1;
			Item.shoot = GetProjectileType();

			var texture = TextureAssets.Item[Type];

			if (texture is not null)
			{
				Item.width = texture.Width();
				Item.height = texture.Height();
			}

			SafeSetDefaults();
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Lighting.AddLight(player.itemLocation, lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}
	}

	public abstract class LuminiteToolProjectile : ModProjectile
	{
		private readonly int projectileCloneType;

		// ...

		public LuminiteToolProjectile(int projectileCloneType)
		{
			this.projectileCloneType = projectileCloneType;
		}

		// ...

		public override string GlowTexture => Texture + "_Glow";

		public sealed override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(name);
		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(projectileCloneType);
			Projectile.glowMask = -1;

			var texture = TextureAssets.Projectile[Type];

			if (texture is not null)
			{
				Projectile.width = texture.Width();
				Projectile.height = texture.Height();
			}
		}

		public sealed override void AI()
		{
			var owner = Main.player[Projectile.owner];
			Projectile.rotation += MathHelper.PiOver2 * -owner.direction * owner.gravDir;
		}
	}
}