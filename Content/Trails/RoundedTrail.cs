using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace OrchidMod.Content.Trails
{
    public class RoundedTrail : SimpleTrail
    {
        protected uint _smoothness;

        public RoundedTrail(Entity target, int length, WidthDelegate width, ColorDelegate color, Effect effect = null, BlendState blendState = null, uint smoothness = 10) : base(target, length, width, color, effect, blendState)
        {
            _smoothness = smoothness;
        }

        protected override int ExtraTrianglesCount => (int)_smoothness;

        protected override void CreateTipMesh(Vector2 normal, Color color)
        {
            float angle = MathHelper.Pi / _smoothness;
            float currentAngle = 0;

            for (int i = 0; i < _smoothness; i++)
            {
                AddVertex(_points[0], color, new Vector2(0.5f, 0.5f));
                AddVertex(_points[0] + normal.RotatedBy(currentAngle), color, new Vector2(1, 1));

                currentAngle += angle;

                AddVertex(_points[0] + normal.RotatedBy(currentAngle), color, new Vector2(0, 1));
            }
        }
    }
}