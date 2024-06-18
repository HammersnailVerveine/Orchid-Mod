using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria.Audio;
using OrchidMod.Utilities;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class AvalancheScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item28;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<AvalancheProjectile>();
			Element = ShamanElement.WATER;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void CatalystSummonRelease(Player player, Projectile projectile)
		{
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed * 1.5f;
			int type = ModContent.ProjectileType<AvalancheProjectileAlt>();
			NewShamanProjectile(player, (EntitySource_ItemUse)player.GetSource_ItemUse(Item), projectile.Center, velocity, type, Item.damage * 10, Item.knockBack);
			SoundEngine.PlaySound(SoundID.Item28, projectile.Center);
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 2) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Player player = Main.player[projectile.owner];
					Vector2 velocity = Vector2.Normalize(target - projectile.Center) * Item.shootSpeed;
					Vector2 position = projectile.Center + Vector2.UnitY.RotateRandom(MathHelper.Pi) * Main.rand.NextFloat(32f);
					NewShamanProjectile(player, (EntitySource_ItemUse)player.GetSource_ItemUse(Item), position, velocity, Item.shoot, projectile.damage, projectile.knockBack);
				}
			}
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newPosition = position + Vector2.UnitY.RotateRandom(MathHelper.Pi) * Main.rand.NextFloat(32f);
			NewShamanProjectile(player, source, newPosition, velocity, type, damage, knockback);
			return false;
		}
	}
}

namespace OrchidMod.Content.Shaman.Projectiles
{
	public class AvalancheProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 25;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(6))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				Color color = new Color(100, 200, 255);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.175f, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
	public class AvalancheProjectileAlt : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 40;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 15)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 15) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				Color color = new Color(100, 200, 255);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.1f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}

