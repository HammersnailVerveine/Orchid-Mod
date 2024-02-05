using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class FeatherScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 13;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<FeatherScepterProjectile>();
			Element = ShamanElement.AIR;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 3) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Vector2 velocity = target - projectile.Center;
					velocity.Normalize();
					velocity *= Item.shootSpeed;
					NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack);

					if (Main.player[projectile.owner].GetModPlayer<OrchidShaman>().CountShamanicBonds() > 0)
					{
						Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15)) / 2f;
						NewShamanProjectileFromProjectile(projectile, newVelocity, Item.shoot, projectile.damage, projectile.knockBack);
					}
				}
			}
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (modPlayer.CountShamanicBonds() > 0)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(0.3f, 0.7f);
				NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}

			NewShamanProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(10f)), type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<HarpyTalon>(), 2)
			.AddIngredient(ItemID.Feather, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

namespace OrchidMod.Content.Shaman.Projectiles
{
	public class FeatherScepterProjectile : OrchidModShamanProjectile
	{
		private bool rapidFade = false;

		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 200;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			projectileTrail = true;
			Projectile.alpha = 255;
		}

		public override void SafeAI()
		{
			Projectile.velocity = Projectile.velocity * 0.95f;

			if (TimeSpent < 10) Projectile.alpha -= 25;

			if (Projectile.timeLeft < 85)
				Projectile.alpha += 3;

			if (Projectile.timeLeft == 130)
			{
				Projectile.damage += 5;
				int type = ModContent.ProjectileType<FeatherScepterProjectileSplash>();
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, 0, 0f, Projectile.owner);
			}

			if (Projectile.timeLeft < 150)
			{
				Projectile.rotation += Projectile.ai[0];
				if (Math.Abs(Projectile.ai[0]) < 0.8f) Projectile.ai[0] += 0.01f;
				if (Projectile.timeLeft > 130) Projectile.height++;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}

			if (rapidFade)
				Projectile.alpha += 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y = 0f;
			Projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}

	public class FeatherScepterProjectileSplash : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 13;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0) Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 5) colorMult *= Projectile.timeLeft / 5f;
			Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult * 0.65f, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
