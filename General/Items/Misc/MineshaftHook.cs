using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Misc
{
	internal class MineshaftHook : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Escape Rope");
			Tooltip.SetDefault("Can only shoot upwards");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 12f;
			Item.shoot = ProjectileType<MineshaftHookProjectile>();
		}
	}

	internal class MineshaftHookProjectile : ModProjectile
	{
		private bool initialized = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Escape Rope");
		}

		public override void SetDefaults()
		{

			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				Projectile.velocity.X = 0f;
				Projectile.velocity.Y = -12f;
				this.initialized = true;
			}
		}

		public override float GrappleRange()
		{
			return 400f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 2;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 12f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 8;
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = Projectile.DirectionTo(player.Center);
			float hangDist = 1f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 distToProj = playerCenter - Projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= 8f;
				center += distToProj;
				distToProj = playerCenter - center;
				distance = distToProj.Length();
				Color drawColor = lightColor;

				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("General/Items/Misc/MineshaftHookChain").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 10, 8), drawColor, projRotation,
					new Vector2(10 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
