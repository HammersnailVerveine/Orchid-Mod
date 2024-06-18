using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.Graphics.Primitives;
using OrchidMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod.Content.Example
{
	[Autoload(false)]
	public class ExampleProjectile : ModProjectile, IDrawOnDifferentLayers
	{
		private PrimitiveStrip trail;

		public override void OnSpawn(IEntitySource source)
		{
			trail = new PrimitiveStrip
			(
				width: progress => 26 * (1 - progress),
				color: progress => Color.Lerp(Color.White, Color.Red, progress),
				effect: new IPrimitiveEffect.Default(texture: OrchidAssets.GetExtraTexture(19), multiplyColorByAlpha: true),
				headTip: new IPrimitiveTip.Rounded(smoothness: 15),
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