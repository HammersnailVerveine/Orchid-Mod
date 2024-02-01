using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class SightScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 25;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 4;
			Item.useAnimation = 32;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.autoReuse = true;
			Item.shootSpeed = 5.25f;
			//Item.shoot = ModContent.ProjectileType<Projectiles.SightScepterProj>();
			Item.UseSound = SoundID.Item15;

			this.Element = ShamanElement.AIR;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Light Concentrator");
			/* Tooltip.SetDefault("Channels a beam of prismatic energy"
							  + "\nHaving 4 or more active shamanic bonds will drastically increase the weapon damage and range"); */
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (modPlayer.CountShamanicBonds() > 3)
				damage *= 1.5f;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddIngredient(ItemID.SoulofSight, 20)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		public override void ExtraAICatalyst(Projectile projectile, bool after)
		{
			if (!after) return;

			Lighting.AddLight(projectile.Center, Main.DiscoColor.ToVector3() * 0.15f);
		}

		public override bool PreDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor)
		{
			Texture2D texture;
			Vector2 position;
			Color color;

			OrchidModProjectile.SetSpriteBatch(spriteBatch, blendState: BlendState.Additive);
			{
				/*if (player.itemAnimation > 0)
				{
					var progress = 1 - player.itemAnimation / (float)player.itemAnimationMax;
					var scale = projectile.scale * 0.5f * OrchidUtils.MultiLerp<float>(MathHelper.Lerp, progress, 0, .8f, 1, .8f, .4f, 0);

					texture = OrchidAssets.GetExtraTexture(15).Value;
					position = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + new Vector2(MathHelper.Lerp(20f, -20f, progress), 0).RotatedBy(projectile.ai[0]);
					color = Main.DiscoColor * 0.8f;
					spriteBatch.Draw(texture, position, new Rectangle(0, 0, 180, 30), color, projectile.ai[0] + MathHelper.PiOver2, new Vector2(90, 30), scale, SpriteEffects.None, 0);
				}*/

				texture = OrchidAssets.GetExtraTexture(16).Value;
				position = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
				color = Main.DiscoColor * 0.55f;
				spriteBatch.Draw(texture, position, null, color, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);

			}
			OrchidModProjectile.SetSpriteBatch(spriteBatch);

			lightColor = Color.White;
			return true;
		}

		public override void PostDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor)
		{
			/*if (player.itemAnimation > 0)
			{
				OrchidModProjectile.SetSpriteBatch(spriteBatch, blendState: BlendState.Additive);

				var texture = OrchidAssets.GetExtraTexture(15).Value;
				var progress = 1 - player.itemAnimation / (float)player.itemAnimationMax;
				var position = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + new Vector2(MathHelper.Lerp(20f, -20f, progress), 0).RotatedBy(projectile.ai[0]);
				var color = Main.DiscoColor * 0.8f;
				var scale = projectile.scale * 0.5f * OrchidUtils.MultiLerp<float>(MathHelper.Lerp, progress, 0, .8f, 1, .8f, .4f, 0);

				spriteBatch.Draw(texture, position, new Rectangle(0, 30, 180, 30), color, projectile.ai[0] + MathHelper.PiOver2, new Vector2(90, 0), scale, SpriteEffects.None, 0);

				OrchidModProjectile.SetSpriteBatch(spriteBatch);
			}*/
		}
	}
}

