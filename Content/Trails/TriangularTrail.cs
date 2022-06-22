using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace OrchidMod.Content.Trails
{ /* SP
    public class TriangularTrail : SimpleTrail
    {
        protected float _tipLength;

        public TriangularTrail(Entity target, int length, WidthDelegate width, ColorDelegate color, Effect effect = null, bool additive = false, float? tipLength = null) : base(target, length, width, color, effect, additive)
        {
            _tipLength = tipLength ?? -1;
        }

        protected override int ExtraTrianglesCount => 1;

        protected override void CreateTipMesh(Vector2 normal, Color color)
        {
            float length = _tipLength >= 0 ? _tipLength : (normal.Length());

            AddVertex(_points[0] - Vector2.Normalize(normal).RotatedBy(-MathHelper.PiOver2) * length, color, new Vector2(0, 0.5f));
            AddVertex(_points[0] - normal, color, new Vector2(0, 0));
            AddVertex(_points[0] + normal, color, new Vector2(0, 1));
        }
    }*/
} 