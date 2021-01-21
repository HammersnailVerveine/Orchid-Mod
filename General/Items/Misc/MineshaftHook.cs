using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Misc
{
	internal class MineshaftHook : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Escape Rope");
		  Tooltip.SetDefault("Can only shoot upwards");
		}

		public override void SetDefaults() {
			item.CloneDefaults(ItemID.AmethystHook);
			item.shootSpeed = 12f;
			item.shoot = ProjectileType<MineshaftHookProjectile>();
		}
	}

	internal class MineshaftHookProjectile : ModProjectile
	{
		private bool initialized = false;
		
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Escape Rope");
		}

		public override void SetDefaults() {

			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}
		
		public override void AI() {
			if (!this.initialized) {
				projectile.velocity.X = 0f;
				projectile.velocity.Y = -12f;
				this.initialized = true;
			}
		}

		public override float GrappleRange() {
			return 400f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) {
			numHooks = 2;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed) {
			speed = 12f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed) {
			speed = 8;
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY) {
			Vector2 dirToPlayer = projectile.DirectionTo(player.Center);
			float hangDist = 1f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
			Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 center = projectile.Center;
			Vector2 distToProj = playerCenter - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance)) {
				distToProj.Normalize();
				distToProj *= 8f;
				center += distToProj;
				distToProj = playerCenter - center;
				distance = distToProj.Length();
				Color drawColor = lightColor;

				spriteBatch.Draw(mod.GetTexture("General/Items/Misc/MineshaftHookChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 10, 8), drawColor, projRotation,
					new Vector2(10 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
