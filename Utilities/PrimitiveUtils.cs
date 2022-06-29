using Microsoft.Xna.Framework;
using OrchidMod.Common.Graphics.Primitives;
using System.Linq;
using Terraria;

namespace OrchidMod.Utilities
{
    public static class PrimitiveUtils
    {
        public static void UpdatePointsAsSimpleTrail(this PrimitiveStrip strip, Vector2 currentPosition, uint maxPoints, float? maxLength = null)
        {
            ref var points = ref strip.Points;

            if (Main.gamePaused) return;
            if (points.Any() && points.First().Equals(currentPosition)) return;

            points.Insert(0, currentPosition);

            if (points.Count > maxPoints) points.Remove(points.Last());
            if (points.Count <= 1 || maxLength == null) return;

            var length = 0f;
            var lastIndex = -1;

            for (int i = 1; i < points.Count; i++)
            {
                float dist = Vector2.Distance(points[i], points[i - 1]);
                length += dist;

                if (length > maxLength)
                {
                    lastIndex = i;
                    length -= dist;
                    break;
                }
            }

            if (lastIndex < 0) return;

            var vector = Vector2.Normalize(points[lastIndex] - points[lastIndex - 1]) * (maxLength - length) ?? Vector2.Zero;
            points.RemoveRange(lastIndex, points.Count - lastIndex);
            points.Add(points[lastIndex - 1] + vector);
        }
    }
}
