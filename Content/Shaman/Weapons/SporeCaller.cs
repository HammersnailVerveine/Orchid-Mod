using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria.DataStructures;
using System;
using OrchidMod.Utilities;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class SporeCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item65;
			Item.shootSpeed = 10.5f;
			Item.shoot = ModContent.ProjectileType<SporeCallerProjectile>();
			Element = ShamanElement.AIR;
			catalystType = ShamanCatalystType.ROTATE;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			NewShamanProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.5f, 1f), type, damage, knockback);
			if (Main.rand.NextBool(2)) NewShamanProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(180)) * Main.rand.NextFloat(0.25f, 0.5f), type, damage, knockback);
			return false;
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 3) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Vector2 velocity = Vector2.Normalize(target - projectile.Center).RotatedByRandom(MathHelper.ToRadians(30)) * Item.shootSpeed;
					NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack, ai1: 1f);
				}
			}
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer)
		{
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile.type == Item.shoot && projectile.owner == player.whoAmI && projectile.active)
				{
					projectile.ai[1] = 1f;
				}
			}
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.Stinger, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}
}


namespace OrchidMod.Content.Shaman.Projectiles
{
	public class SporeCallerProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Target => (int)Projectile.ai[0] - 1;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Main.rand.NextBool(3))
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);
			}

			if (Projectile.ai[2] == 0) Projectile.ai[2] = Main.rand.NextFloat(-0.1f, 0.1f);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(6))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PoisonStaff);
				dust.noGravity = true;
			}

			if (Projectile.ai[1] > 0f)
			{
				if (Target > -1)
				{
					NPC target = Main.npc[Target];
					if (!target.active && Projectile.timeLeft > 20) Projectile.timeLeft = 20;
					Vector2 newVelocity = Vector2.Normalize(target.Center - Projectile.Center).RotatedBy(Math.Sin(TimeSpent * 0.1f * Math.Sign(Projectile.ai[2])) * 0.5f);
					Projectile.velocity = Projectile.velocity * 0.8f + newVelocity;
				}
				else
				{
					NPC closestTarget = null;
					float distanceClosest = 1200f;
					foreach (NPC npc in Main.npc)
					{
						float distance = Projectile.Center.Distance(npc.Center);
						if (IsValidTarget(npc) && distance < distanceClosest)
						{
							closestTarget = npc;
							distanceClosest = distance;
						}
					}

					if (closestTarget != null && IsLocalOwner)
					{
						Projectile.ai[0] = closestTarget.whoAmI + 1;
						Projectile.netUpdate = true;
						if (Projectile.timeLeft < 60) Projectile.timeLeft = 60;
					}
				}
			}
			else Projectile.velocity *= 0.98f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 20) colorMult *= Projectile.timeLeft / 20f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureAlt, drawPosition, null, Color.White * 0.15f * (i + 1) * colorMult, OldRotation[i] + TimeSpent * Projectile.ai[2], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.2f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPositionMain = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPositionMain, null, lightColor * 3f * colorMult, Projectile.rotation + TimeSpent * Projectile.ai[2], TextureMain.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
