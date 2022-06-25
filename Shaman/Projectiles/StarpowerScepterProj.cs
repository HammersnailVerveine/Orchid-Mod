using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class StarpowerScepterProj : OrchidModShamanProjectile
	{
		private Color mainColor = Color.White;
		private float startRotation = 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");

			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1; // Don't delete it, pls
		}

		public override void OnSpawn()
		{
			// I hate it
			// No u
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int newCrit = 10 * modPlayer.GetNbShamanicBonds() + (int)player.GetCritChance<ShamanDamageClass>() + player.inventory[player.selectedItem].crit;
			OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			modProjectile.baseCritChance = newCrit;

			switch (Main.rand.Next(3))
			{
				case 1:
					mainColor = new Color(231, 120, 255, 100);
					break;
				case 2:
					mainColor = new Color(120, 247, 255, 100);
					break;
				default:
					mainColor = new Color(215, 255, 120, 100);
					break;
			}

			Projectile.ai[0] = 0.35f;
			Projectile.ai[1] = 0; // Death Type
			startRotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.Length() + startRotation;
			Projectile.velocity *= 0.95f;
			Projectile.scale = MathHelper.SmoothStep(1.4f, 0f, Projectile.ai[0]);

			if (Projectile.timeLeft < 25 && Projectile.ai[1] == 0) Projectile.ai[1] = 2;
			if (Projectile.ai[1] > 0) DeathFunction();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				// Light Effect
				Texture2D lightTexture = OrchidAssets.GetExtraTexture(11).Value;
				Vector2 drawPosition = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition;
				spriteBatch.Draw(lightTexture, drawPosition, null, mainColor, Projectile.rotation, lightTexture.Size() * 0.5f, Projectile.scale * 0.5f, SpriteEffects.None, 0);

				// Lens Flare
				if (Projectile.timeLeft < 15 && Projectile.ai[1] == 2)
				{
					Texture2D texture = OrchidAssets.GetExtraTexture(6).Value;
					float size = MathHelper.SmoothStep(1, 0, Math.Abs(1f - Projectile.timeLeft * 2 / 15)) * 0.9f;
					Color color = mainColor;
					color.A = 200;
					spriteBatch.Draw(texture, drawPosition, null, color, 0f, texture.Size() * 0.5f, size, SpriteEffects.None, 0);
				}

				// Trail
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					drawPosition = Projectile.oldPos[k] + Projectile.Size * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
					float num = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Color color = mainColor * num * 0.8f;
					spriteBatch.Draw(OrchidAssets.GetExtraTexture(0).Value, drawPosition, null, color, Projectile.oldRot[k], OrchidAssets.GetExtraTexture(0).Size() * .5f, Projectile.scale * num, SpriteEffects.None, 0f);
				}
			}
			SetSpriteBatch(spriteBatch: spriteBatch);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Vector2 drawPosition = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture, drawPosition, null, Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Color.White), Projectile.rotation, texture.Size() * .5f, Projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.timeLeft = 24;
			Projectile.ai[1] = 1;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Projectile.timeLeft = 24;
			Projectile.ai[1] = 1;
		}

		private void DeathFunction()
		{
			Projectile.damage = 0;
			Projectile.tileCollide = false;

			if (Projectile.timeLeft < 25)
			{
				if (Projectile.timeLeft >= 20) Projectile.ai[0] -= 0.035f;
				else Projectile.ai[0] += 0.06f;
				if (Projectile.ai[0] > 1f) Projectile.Kill();
			}
		}
	}
}