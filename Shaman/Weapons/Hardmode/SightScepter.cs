using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class SightScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 48;
			item.height = 48;
			item.useTime = 4;
			item.useAnimation = 32;
			item.knockBack = 0f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 5.25f;
			item.shoot = ModContent.ProjectileType<Projectiles.SightScepterProj>();
			item.UseSound = SoundID.Item15;

			this.empowermentType = 3;
			this.energy = 3;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Light Concentrator");
			Tooltip.SetDefault("Channels a beam of prismatic energy"
							  + "\nHaving 4 or more active shamanic bonds will drastically increase the weapon damage and range");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			/*var dust = Dust.NewDustPerfect(position, DustID.GoldFlame, Vector2.One * 5);
			dust.noGravity = true;
			*/
			return base.SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			mult *= modPlayer.shamanDamage;

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 3)
			{
				mult *= 1.5f;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

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
					var scale = projectile.scale * 0.5f * OrchidHelper.GradientValue<float>(MathHelper.Lerp, progress, 0, .8f, 1, .8f, .4f, 0);

					texture = OrchidHelper.GetExtraTexture(15);
					position = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + new Vector2(MathHelper.Lerp(20f, -20f, progress), 0).RotatedBy(projectile.ai[0]);
					color = Main.DiscoColor * 0.8f;
					spriteBatch.Draw(texture, position, new Rectangle(0, 0, 180, 30), color, projectile.ai[0] + MathHelper.PiOver2, new Vector2(90, 30), scale, SpriteEffects.None, 0);
				}*/

				texture = OrchidHelper.GetExtraTexture(16);
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

				var texture = OrchidHelper.GetExtraTexture(15);
				var progress = 1 - player.itemAnimation / (float)player.itemAnimationMax;
				var position = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + new Vector2(MathHelper.Lerp(20f, -20f, progress), 0).RotatedBy(projectile.ai[0]);
				var color = Main.DiscoColor * 0.8f;
				var scale = projectile.scale * 0.5f * OrchidHelper.GradientValue<float>(MathHelper.Lerp, progress, 0, .8f, 1, .8f, .4f, 0);

				spriteBatch.Draw(texture, position, new Rectangle(0, 30, 180, 30), color, projectile.ai[0] + MathHelper.PiOver2, new Vector2(90, 0), scale, SpriteEffects.None, 0);

				OrchidModProjectile.SetSpriteBatch(spriteBatch);
			}*/
		}
	}
}

