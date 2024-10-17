using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidShapeshifter : ModPlayer
	{
		public OrchidPlayer modPlayer;
		public ShapeshifterShapeshiftAnchor ShapeshiftAnchor;
		public OrchidModShapeshifterShapeshift Shapeshift;

		public int GetShapeshifterDamage(float damage) => (int)(Player.GetDamage<ShapeshifterDamageClass>().ApplyTo(damage) + Player.GetDamage(DamageClass.Generic).ApplyTo(damage) - damage);
		public int GetShapeshifterCrit(int additionalCritChance) => (int)(Player.GetCritChance<ShapeshifterDamageClass>() + Player.GetCritChance<GenericDamageClass>() + additionalCritChance);

		// Can be edited by gear


		// Set effects, accessories, misc


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

		public override void PostUpdate()
		{
			if (ShapeshiftAnchor != null && ShapeshiftAnchor.Projectile.active)
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
		}

		public override void OnRespawn()
		{
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
		}
	}
}
