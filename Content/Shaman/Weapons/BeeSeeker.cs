using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class BeeSeeker : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<BeeSeekerProjectile>();
			this.Element = ShamanElement.WATER;
			this.CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void CatalystSummonRelease(Player player, Projectile projectile)
		{

			int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(10);
			EntitySource_ItemUse source = (EntitySource_ItemUse)player.GetSource_ItemUse(Item);
			for (int i = 0; i < 15; i ++)
			{
				if (player.strongBees && Main.rand.NextBool(2))
					Projectile.NewProjectile(source, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, player.whoAmI);
				else
					Projectile.NewProjectile(source, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.Bee, dmg, 0f, player.whoAmI);
			}

			SoundEngine.PlaySound(SoundID.Item97, player.Center);
		}
	}
}


namespace OrchidMod.Content.Shaman.Projectiles
{
	public class BeeSeekerProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 240;
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

			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Honey2, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}

			Projectile.velocity.Y += 0.15f;
			Projectile.velocity.X *= 0.99f;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.LiquidsHoneyWater, Projectile.Center);
			int type = ModContent.ProjectileType<BeeSeekerProjectileSplash>();
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, 0, 0f, Projectile.owner);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;

			Vector2 drawPositionMain = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPositionMain, null, lightColor * 0.8f, Projectile.rotation + TimeSpent * 0.1f, TextureMain.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0f);

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * 0.1f * i * colorMult, OldRotation[i] + TimeSpent * 0.1f * (i % 2 == 0 ? 1 : -1), TextureMain.Size() * 0.5f, Projectile.scale * i * 0.175f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}

	public class BeeSeekerProjectileSplash : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 10;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0)
			{
				Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
				Projectile.ai[1] = Main.rand.NextFloat(MathHelper.Pi);

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Honey, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.velocity *= 1.25f;
					dust.noGravity = true;
				}

				if (Main.LocalPlayer.Hitbox.Intersects(Projectile.Hitbox)) Main.LocalPlayer.AddBuff(BuffID.Honey, 600);
			}
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 5) colorMult *= Projectile.timeLeft / 5f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult * 0.7f, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.18f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult * 0.25f, Projectile.rotation + MathHelper.PiOver2, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.12f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}

