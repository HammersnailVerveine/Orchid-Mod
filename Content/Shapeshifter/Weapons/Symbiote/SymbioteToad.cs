using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Symbiote
{
	public class SymbioteToad : OrchidModShapeshifterShapeshift
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 55, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit24;
			Item.useTime = 30;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 53;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Symbiote;
			Grounded = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

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
			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			// MISC EFFECTS & ANIMATION

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{ // Left movement
				TryAccelerate(ref intendedVelocity, -1.75f, speedMult, 0.1f);
				projectile.direction = -1;
				projectile.spriteDirection = -1;
			}
			else if (anchor.IsInputRight && !anchor.IsInputLeft)
			{ // Right movement
				TryAccelerate(ref intendedVelocity, 1.75f, speedMult, 0.1f);
				projectile.direction = 1;
				projectile.spriteDirection = 1;
			}
			else
			{ // Both keys pressed = no movement
				intendedVelocity.X *= 0.7f;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

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
	}
}