using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class AmberScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 26;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.75f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectileAmber>();
			Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer)
		{
			shamanPlayer.ShamanEarthBond += 300;
			shamanPlayer.modPlayer.TryHeal(20);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Amber, 8)
			.AddIngredient(ItemID.FossilOre, 15)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

namespace OrchidMod.Content.Shaman.Projectiles
{
	public class GemScepterProjectileAmber : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0) Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);

			if (Main.rand.NextBool(5))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			if (Projectile.timeLeft < 30 || Projectile.penetrate < 1)
			{
				Projectile.velocity *= 0.9f;
			}

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 1) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Projectile.timeLeft > 30) Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			int type = ModContent.ProjectileType<GemScepterBreak>();
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * 0.15f * (i + 1), OldRotation[i] + (float)Math.Pow(TimeSpent, 2.2) * 0.001f, TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * (0.2f + TimeSpent * 0.003f), SpriteEffects.None, 0f);
			}

			return false;
		}
	}

	public class GemScepterBreak : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;
		private Color DrawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 15;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0)
			{
				ResetIFrames();
				Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
				Projectile.ai[1] = Main.rand.NextFloat(MathHelper.Pi);

				DrawColor = Color.White;

				switch (Projectile.ai[0])
				{
					case 1: // Amber
						DrawColor = new Color(207, 109, 0);
						break;
					case 2: // Amethist
						DrawColor = new Color(207, 0, 236);
						break;
					case 3: // Topaz
						DrawColor = new Color(255, 221, 62);
						break;
					case 4: // Sapphire
						DrawColor = new Color(107, 210, 252);
						break;
					case 5: // Emerald
						DrawColor = new Color(10, 152, 98);
						break;
					case 6: // Ruby
						DrawColor = new Color(238, 51, 53);
						break;
					case 7: // Diamond
						DrawColor = new Color(223, 230, 238);
						break;
					default: 
						break;
				}
				return;
			}

			Projectile.friendly = false;
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 5) colorMult *= Projectile.timeLeft / 5f;
			Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult * 0.5f, Projectile.rotation + MathHelper.PiOver2, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAlt, drawPosition, null, DrawColor * colorMult * 0.8f, Projectile.ai[1], TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
