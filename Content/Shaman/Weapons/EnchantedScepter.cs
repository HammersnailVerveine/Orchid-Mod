using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class EnchantedScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<EnchantedScepterProjectile>();
			Element = ShamanElement.FIRE;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
	}
}

namespace OrchidMod.Content.Shaman.Projectiles
{
	public class EnchantedScepterProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Target => (int)Projectile.ai[0] - 1;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.tileCollide = false;
		}

		public override void SafeAI()
		{
			if (Projectile.ai[1] == 0)
			{
				Projectile.ai[1] = Main.rand.Next(2) + 1;
				Projectile.tileCollide = Projectile.ai[2] == 0;
			}

			if (TimeSpent == 30 && Projectile.ai[2] > 0f)
			{
				Projectile.ai[2] = -1f;
				Projectile.timeLeft += 30;

				if (Projectile.owner == Main.myPlayer)
				{
					NPC closestTarget = null;
					float distanceClosest = 900f;
					foreach (NPC npc in Main.npc)
					{
						float distance = Projectile.Center.Distance(npc.Center);
						if (distance < distanceClosest && npc.active && !npc.friendly && !npc.CountsAsACritter)
						{
							closestTarget = npc;
							distanceClosest = distance;
						}
					}

					if (closestTarget != null && IsLocalOwner)
					{
						Projectile.ai[0] = closestTarget.whoAmI + 1;
						Projectile.netUpdate = true;
					}
				}
			}

			if (Target > -1)
			{
				NPC target = Main.npc[Target];

				if (!target.active) Projectile.Kill();
				Vector2 newVelocity = Vector2.Normalize(target.Center - Projectile.Center) * 0.65f;
				Projectile.velocity = Projectile.velocity * 0.965f + newVelocity;
				Projectile.rotation = newVelocity.ToRotation();
			}
			else Projectile.rotation = Projectile.velocity.ToRotation();

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(4))
			{
				int dustType = Main.rand.Next(3);
				if (dustType == 0) dustType = 15;
				if (dustType == 1) dustType = 57;
				if (dustType == 2) dustType = 58;

				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, Scale : Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if ((target.whoAmI != Target && Target >= 0) || Projectile.ai[2] > 0f) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (IsLocalOwner && modPlayer.CountShamanicBonds() > 0 && Projectile.ai[2] == 0f)
			{
				NPC closestTarget = null;
				float distanceClosest = 900f;
				foreach (NPC npc in Main.npc)
				{
					float distance = target.Center.Distance(npc.Center);
					if (distance < distanceClosest && npc.whoAmI != target.whoAmI && npc.active && !npc.friendly && !npc.CountsAsACritter)
					{
						closestTarget = npc;
						distanceClosest = distance;
					}
				}

				int targetID = closestTarget == null ? -1 : closestTarget.whoAmI + 1;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.velocity, Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(180)) * 8f, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, targetID, 0f, 1f);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 15) colorMult *= Projectile.timeLeft / 15f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				Color color = new Color(125, 155, 255);
				if (Projectile.ai[1] == 2) color = new Color(200, 66, 125);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.175f, SpriteEffects.None, 0f);;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i] * 1.2f, TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
