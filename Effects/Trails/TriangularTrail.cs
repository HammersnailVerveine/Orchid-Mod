using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace OrchidMod.Effects.Trails
{
    public class TriangularTrail : SimpleTrail
    {
        protected int _tipLength;

        public TriangularTrail(int length, WidthDelegate width, ColorDelegate color, Effect effect = null, int? tipLength = null) : base(length, width, color, effect)
        {
            _tipLength = tipLength ?? -1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_points.Count <= 1) return;

            this.PreDraw(spriteBatch);

            float progress = 0f;
            float currentWidth = _width?.Invoke(progress) ?? 0;
            Color currentColor = (_color?.Invoke(progress) ?? Color.White) * _dissolveProgress;

            Vector2 normal = (_points[1] - _points[0]).SafeNormalize(Vector2.Zero) * currentWidth / 2f;
            this.AddVertex(_points[0] - normal * 0.5f, currentColor, new Vector2(0, 0.5f));
            normal = normal.RotatedBy(MathHelper.PiOver2);

            this.AddVertex(_points[0] - normal, currentColor, new Vector2(progress, 1));
            this.AddVertex(_points[0] + normal, currentColor, new Vector2(progress, 0));

            for (int i = 1; i < _points.Count; i++)
            {
                progress += Vector2.Distance(_points[i], _points[i - 1]) / this.Length;

                currentWidth = _width?.Invoke(progress) ?? 0;
                currentColor = (_color?.Invoke(progress) ?? Color.White) * _dissolveProgress;
                normal = (_points[i] - _points[i - 1]).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * currentWidth / 2f;

                this.AddVertex(_points[i] - normal, currentColor, new Vector2(progress, 1));
                this.AddVertex(_points[i] + normal, currentColor, new Vector2(progress, 0));
            }

            _effect.Parameters["transformMatrix"].SetValue(Primitives.GetTransformMatrix());

            var graphics = Main.instance.GraphicsDevice;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, _vertices.ToArray(), 0, (_points.Count - 1) * 2 + 1);
            }

            this.PostDraw(spriteBatch);
        }
    }
}
