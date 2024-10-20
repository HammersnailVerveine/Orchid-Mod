using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidShapeshifter : ModPlayer
	{
		public OrchidPlayer modPlayer;
		public ShapeshifterShapeshiftAnchor ShapeshiftAnchor;
		public OrchidModShapeshifterShapeshift Shapeshift;
		public bool IsShapeshifted => ShapeshiftAnchor != null && ShapeshiftAnchor.Projectile.active;

		public int GetShapeshifterDamage(float damage) => (int)(Player.GetDamage<ShapeshifterDamageClass>().ApplyTo(damage) + Player.GetDamage(DamageClass.Generic).ApplyTo(damage) - damage);
		public int GetShapeshifterCrit(int additionalCritChance) => (int)(Player.GetCritChance<ShapeshifterDamageClass>() + Player.GetCritChance<GenericDamageClass>() + additionalCritChance);

		// Can be edited by gear


		// Set effects, accessories, misc

		public int ShapeshifterSageFoxSpeed = 0;


		// Dynamic gameplay and UI fields

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			if (ShapeshiftAnchor != null)
			{
				foreach (var layer in PlayerDrawLayerLoader.DrawOrder)
				{
					layer.Hide();
				}
			}
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}

		public override void ResetEffects()
		{
			if (ShapeshiftAnchor != null && ShapeshiftAnchor.Projectile.active)
			{
				if (Player.mount.Active || Player.grappling[0] >= 0)
				{ // Disable the shapeshift if the player is mounted or uses a hook
					ShapeshiftAnchor.Projectile.Kill();
				}
			}
		}

		public override void PostUpdateEquips()
		{
			// Misc Effects that should be called before Shapeshifter Core mechanics (eg : stat changes that should affec the shapeshifted player)

			if (ShapeshifterSageFoxSpeed > 0)
			{
				ShapeshifterSageFoxSpeed--;
				Player.moveSpeed += ShapeshifterSageFoxSpeed * 0.003f;
			}
		}

		public override void PostUpdate()
		{
			// Shapeshifter core stuff

			if (IsShapeshifted)
			{ // Runs the shapeshift AI and adjust player position accordingly
				Player.width = Shapeshift.ShapeshiftWidth;
				Player.height = Shapeshift.ShapeshiftHeight;

				Projectile projectile = ShapeshiftAnchor.Projectile;
				Shapeshift.ShapeshiftAnchorAI(projectile, ShapeshiftAnchor, Player, this);
				ShapeshiftAnchor.ExtraAI();

				Player.velocity = projectile.velocity;
				Player.Center = projectile.Center + projectile.velocity;
			}
			else if (ShapeshiftAnchor != null || Shapeshift != null)
			{ // Failsafe in case the anchor isn't properly killed
				Player.width = Player.defaultWidth;
				Player.height = Player.defaultHeight;
				Shapeshift = null;
				ShapeshiftAnchor = null;
			}

			// Misc Effects that should be called after shapeshifter core mechanics (eg: that depend of the player width and height to be correct)

			if (ShapeshifterSageFoxSpeed > 0)
			{
				if (Main.rand.NextBool((int)(30 - ShapeshifterSageFoxSpeed / 6f) + 1))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.IceTorch, Scale: Main.rand.NextFloat(0.8f, 1.2f));
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (IsShapeshifted && !Player.noKnockback)
			{ // Player knockback on hit
				ShapeshiftAnchor.Projectile.velocity = new Vector2(3f * hurtInfo.HitDirection, -3f);
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (IsShapeshifted && !Player.noKnockback)
			{ // Player knockback on hit
				ShapeshiftAnchor.Projectile.velocity = new Vector2(3f * hurtInfo.HitDirection, -3f);
			}
		}

		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (IsShapeshifted)
			{
				Shapeshift.ShapeshiftModifyHurt(ref modifiers, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
			}
		}

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if (IsShapeshifted)
			{
				return Shapeshift.ShapeshiftFreeDodge(info, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
			}
			return false;
		}
	}
}
