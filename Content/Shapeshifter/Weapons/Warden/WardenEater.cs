using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenEater : OrchidModShapeshifterShapeshift
	{
		private bool BackwardsAnimation = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 74, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Grass;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 5f;
			Item.damage = 50;
			ShapeshiftWidth = 22;
			ShapeshiftHeight = 22;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			Grounded = false;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = 1;
			projectile.spriteDirection = 1;
			BackwardsAnimation = false;

			if (IsLocalPlayer(player))
			{
				float maxRange = 288f * GetSpeedMult(player, shapeshifter, anchor); // 18 tiles * movespeed
				int projectileType = ModContent.ProjectileType<WardenEaterStem>();
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, 0, 0f, player.whoAmI, Main.rand.Next(1000), maxRange + 8f);
				projectile.ai[0] = maxRange;
				anchor.NeedNetUpdate = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS & ANIMATION

			if (anchor.Timespent % 5 == 0)
			{ // Animation frames
				if (BackwardsAnimation)
				{
					anchor.Frame--;

					if (anchor.Frame <= 0)
					{
						BackwardsAnimation = false;
					}
				}
				else
				{
					anchor.Frame++;

					if (anchor.Frame >= 2)
					{
						BackwardsAnimation = true;
					}
				}
			}

			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			Projectile stem = null;

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<WardenEaterStem>() && proj.owner == player.whoAmI)
				{
					stem = proj;
					break;
				}
			}

			if (stem == null)
			{ // unshift if there is a problem with the stem
				projectile.Kill();
			}
			else
			{ // else rotate according to its position
				projectile.rotation = (stem.Center - projectile.Center).ToRotation() - MathHelper.PiOver2;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;

			// 8 direction movement
			float velocityX = 0f;
			float velocityY = 0f;

			if (anchor.IsInputUp && !anchor.IsInputDown)
			{ // Top movement
				velocityY = -4f;
			}
			else if (anchor.IsInputDown && !anchor.IsInputUp)
			{ // Bottom movement
				velocityY = 4f;
			}
			else
			{ // Both keys pressed or no key pressed = no Y movement
				intendedVelocity.Y *= 0.8f;
			}

			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{ // Left movement
				velocityX = -4f;
			}
			else if (anchor.IsInputRight && !anchor.IsInputLeft)
			{ // Right movement
				velocityX = 4f;
			}
			else
			{ // Both keys pressed or no key pressed = no X movement
				intendedVelocity.X *= 0.8f;
			}

			if (velocityX != 0f && velocityY != 0f)
			{ // diagonal movement, multiply both velocities so the speed isn't faster diagonally
				velocityX *= 0.70725f; // approx
				velocityY *= 0.70725f;
			}

			if (velocityX != 0f)
			{
				TryAccelerate(ref intendedVelocity, velocityX, speedMult, 0.112f);
			}

			if (velocityY != 0f)
			{
				TryAccelerate(ref intendedVelocity, velocityY, speedMult, 0.125f, Yaxis:true);
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			if (stem != null)
			{
				if (stem.Center.Distance(projectile.Center) > projectile.ai[0])
				{ // Keeps the player inside the max range
					Vector2 intendedCenter = stem.Center - Vector2.Normalize(stem.Center - projectile.Center) * projectile.ai[0];
					Vector2 forcedMovement = intendedCenter - projectile.Center;

					Vector2 finalMovement = Vector2.Zero;
					forcedMovement /= 10f;
					for (int i = 0; i < 10; i++)
					{
						finalMovement += Collision.TileCollision(projectile.position + finalMovement, forcedMovement, projectile.width, projectile.height, true, true, (int)player.gravDir);
					}

					projectile.Center += finalMovement;
				}
			}

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > 4)
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<ShapeshifterBlankEffigy>();
			recipe.AddIngredient(ItemID.Vine, 5);
			recipe.AddIngredient(ItemID.JungleSpores, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}