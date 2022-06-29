using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.Graphics.Primitives;
using OrchidMod.Utilities;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod
{
	[Autoload(false)]
	public class ExampleProjectile : ModProjectile, IDrawOnDifferentLayers
	{
		private static Effect trailEffect;
		private PrimitiveStrip trail;

		public override void SetStaticDefaults()
		{
			Main.QueueMainThreadAction(() => trailEffect = EffectLoader.CreateDefaultTrailEffect
			(
				texture: OrchidAssets.GetExtraTexture(type: 19, mode: AssetRequestMode.ImmediateLoad).Value,
				multiplyColorByAlpha: true
			));
		}

		public override void Unload()
		{
			trailEffect = null;
		}

		public override void OnSpawn(IEntitySource source)
		{
			trail = new PrimitiveStrip
			(
				width: progress => 26 * (1 - progress),
				color: progress => MathUtils.MultiLerp(Color.Lerp, progress, Color.White, Color.Blue, Color.Red) * (1 - progress),
				effect: trailEffect,
				headTip: new IPrimitiveStripTip.Rounded(smoothness: 15),
				tailTip: null
			);
		}

		void IDrawOnDifferentLayers.DrawOnDifferentLayers(DrawSystem system)
		{
			trail.UpdatePointsAsSimpleTrail(currentPosition: Projectile.Center, maxPoints: 25, maxLength: 16 * 7);
			system.AddToAlphaBlend(layer: DrawLayers.Tiles, data: trail);
		}
	}
}