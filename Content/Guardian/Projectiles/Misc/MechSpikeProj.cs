using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class MechSpikeProj : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.timeLeft = 180;
			Projectile.friendly = true;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 3;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			lightColor = new Color(1f, 1f, 1f, 0f);
			return true;
		}
	}
}