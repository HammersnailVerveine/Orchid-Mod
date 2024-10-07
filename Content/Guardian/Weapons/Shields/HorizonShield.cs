using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using System;
using System.Collections.Generic;
using OrchidMod.Utilities;
using Terraria.Audio;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class HorizonShield : OrchidModGuardianShield
	{
		public int TimeHeld;
		public bool StoredBlock;
		public Texture2D TextureGlow;
		public Texture2D TextureGlowAlt;
		public float ColorMult;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.width = 42;
			Item.height = 50;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.DD2_FlameburstTowerShot;
			Item.knockBack = 6f;
			Item.damage = 527;
			Item.rare = ItemRarityID.Red;
			Item.useTime = 60;
			Item.shootSpeed = 12f;
			distance = 70f;
			slamDistance = 150f;
			blockDuration = 300;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlowAlt ??= ModContent.Request<Texture2D>(Texture + "_GlowAlt", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			TimeHeld = 0;
		}

		public override void UpdateInventory(Player player)
		{
			if (player.HeldItem.type != Type && OldPosition.Count > 0)
			{
				TimeHeld = 0;
				StoredBlock = false;
				OldPosition = new List<Vector2>();
				OldRotation = new List<float>();
			}

			Item.UseSound = StoredBlock ? SoundID.DD2_ExplosiveTrapExplode : SoundID.DD2_FlameburstTowerShot;
		}

		public override void HoldItemFrame(Player player)
		{
			TimeHeld ++;
		}

		public override void Slam(Player player, Projectile shield)
		{
			if (IsLocalPlayer(player))
			{
				Projectile anchor = GetAnchor(player).Projectile;
				int type = ModContent.ProjectileType<HorizonShieldProj>();
				Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * 15f;
				Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(shield.damage * (StoredBlock ? 3f : 1f)), Item.knockBack, player.whoAmI, StoredBlock ? 1f : 0f);
				StoredBlock = false;
			}
		}

		public override void ExtraAIShield(Projectile projectile)
		{
			if (projectile.ai[1] + projectile.ai[0] > 0)
			{
				ColorMult = (float)Math.Sin(TimeHeld * 0.1f) * 0.25f + 0.75f;

				if (OldPosition.Count > (StoredBlock ? 20 : 15))
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}

				OldPosition.Add(new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)));
				OldRotation.Add(projectile.rotation);

				for (int i = 0; i < OldPosition.Count; i++)
				{
					Vector2 pos = OldPosition[i];
					pos.Y += Main.rand.NextFloat(6.4f);
					pos.X += Main.rand.NextFloat(3f) - 1.5f;
					OldPosition[i] = pos;
				}
			}
			else
			{
				StoredBlock = false;
				ColorMult = (float)Math.Sin(TimeHeld * 0.1f) * 0.125f + 0.35f;

				if (OldPosition.Count > 10)
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}
				else
				{
					OldPosition.Add(new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)));
					OldRotation.Add(projectile.rotation);
				}

				for (int i = 0; i < OldPosition.Count; i++)
				{
					Vector2 pos = OldPosition[i];
					pos.Y += Main.rand.NextFloat(3.15f);
					pos.X += Main.rand.NextFloat(1f) - 0.5f;
					OldPosition[i] = pos;
				}
			}
		}

		public override void Protect(Player player, Projectile shield)
		{
			if (!StoredBlock && IsLocalPlayer(player))
			{
				StoredBlock = true;
				SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, player.Center);
			}
		}

		public override void PostDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor)
		{
			var drawPosition = Vector2.Transform(projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
			var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			spriteBatch.Draw(TextureGlow, drawPosition, null, Color.White * ColorMult, projectile.rotation, TextureGlow.Size() * 0.5f, projectile.scale, effect, 0f);

			Vector2 slamoffset = Vector2.Zero;
			for (int i = 0; i < OldPosition.Count - 1; i++)
			{
				if (projectile.ai[1] > 0)
				{
					slamoffset.X += i * 0.2f;
				}
				Vector2 drawPositionGlow = Vector2.Transform(projectile.Center - (Vector2.UnitY * 6f + OldPosition[i] + slamoffset).RotatedBy(projectile.rotation) - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureGlowAlt, drawPositionGlow, null, new Color(216, 61, 30), OldRotation[i], TextureGlowAlt.Size() * 0.5f, projectile.scale * (i + 1) * 0.08f, effect, 0f);

				SpriteEffects effectFlip = projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				drawPositionGlow = Vector2.Transform(projectile.Center + (Vector2.UnitY * 6f + OldPosition[i] - slamoffset).RotatedBy(projectile.rotation) - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureGlowAlt, drawPositionGlow, null, new Color(59, 88, 204), OldRotation[i] + MathHelper.Pi, TextureGlowAlt.Size() * 0.5f, projectile.scale * (i + 1) * 0.08f, effectFlip, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<HorizonFragment>(18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
