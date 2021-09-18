using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using Terraria;

namespace OrchidMod.Content.Trails
{
    public class SimpleTrail : PrimitiveTrailSystem.Trail
    {
        protected readonly WidthDelegate _width;
        protected readonly ColorDelegate _color;

        public SimpleTrail(Entity target, int length, WidthDelegate width, ColorDelegate color, Effect effect = null, BlendState blendState = null) : base(target, length, effect, blendState)
        {
            _width = width;
            _color = color;
        }

        protected override void CreateMesh()
        {
            float progress = 0f;
            (Color color, Vector2 normal) = GetProgressVariables(progress);

            CreateTipMesh(normal, color);

            for (int i = 1; i < _points.Count; i++)
            {
                float nextProgress = progress + Vector2.Distance(_points[i], _points[i - 1]) / this.Length;
                (Color nextColor, Vector2 nextNormal) = GetProgressVariables(nextProgress, i);

                AddVertex(_points[i - 1] - normal, color, new Vector2(progress, 0));
                AddVertex(_points[i] - nextNormal, nextColor, new Vector2(nextProgress, 0));
                AddVertex(_points[i] + nextNormal, nextColor, new Vector2(nextProgress, 1));

                AddVertex(_points[i - 1] - normal, color, new Vector2(progress, 0));
                AddVertex(_points[i] + nextNormal, nextColor, new Vector2(nextProgress, 1));
                AddVertex(_points[i - 1] + normal, color, new Vector2(progress, 1));

                progress = nextProgress;
                color = nextColor;
                normal = nextNormal;
            }
        }

        protected virtual void CreateTipMesh(Vector2 normal, Color color) { }

        private (Color, Vector2) GetProgressVariables(float progress, int index = 1)
        {
            float width = _width?.Invoke(progress) ?? 0;
            Color color = (_color?.Invoke(progress) ?? Color.White) * _dissolveProgress;
            Vector2 normal = (_points[index] - _points[index - 1]).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width / 2f;
            return (color, normal);
        }
    }
}
