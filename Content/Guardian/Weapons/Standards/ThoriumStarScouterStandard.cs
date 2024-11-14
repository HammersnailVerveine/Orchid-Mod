using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class ThoriumStarScouterStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 35;
			Item.UseSound = SoundID.Item113;
			GuardStacks = 2;
			FlagOffset = 6;
			AuraRange = 20;
			StandardDuration = 1800;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}

		public override Color GetColor()
		{
			return new Color(164, 46, 255);
		}

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			affectedPlayer.GetModPlayer<OrchidGuardian>().GuardianStandardStarScouter = guardian.Player.whoAmI;
			affectedPlayer.GetModPlayer<OrchidGuardian>().GuardianStandardBuffer = true;
			if (reinforced && isLocalPlayer)
			{
				guardian.GuardianStandardStarScouterWarp = true;
			}
			//Dust.NewDustPerfect(guardian.Player.Center + new Vector2(-14 * guardian.Player.direction, -28 * guardian.Player.gravDir) - new Vector2(4, 4), DustID.ShadowbeamStaff);
			return true;
		}

		public override bool DrawCustomFlag(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor, Vector2 drawPosition, float drawRotation)
		{
			GuardianStandardAnchor anchor = projectile.ModProjectile as GuardianStandardAnchor;
			var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D textureUp = ModContent.Request<Texture2D>(FlagUpTexture).Value;
			float oscillation = (float)Math.Sin(projectile.ai[1] * 0.08f) * 2f;
			float intensity = Math.Min(1, projectile.ai[1] / 600f);
			Vector2 offs = new Vector2(oscillation).RotatedBy(drawRotation + MathHelper.PiOver2) - player.velocity * 0.5f;
			offs = offs * intensity + new Vector2(2).RotatedBy(drawRotation + MathHelper.PiOver2) * (1 - intensity);
			spriteBatch.Draw(textureUp, drawPosition + offs, null, lightColor, drawRotation, textureUp.Size() * 0.5f, projectile.scale, effect, 0f);
			if (anchor.Reinforced)
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_FlagGlow").Value, drawPosition + offs, null, Color.White * (intensity + -oscillation * 0.25f), drawRotation, textureUp.Size() * 0.5f, projectile.scale, effect, 0f);
			return true;
		}
	}
}
