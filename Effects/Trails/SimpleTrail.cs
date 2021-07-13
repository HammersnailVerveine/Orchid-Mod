using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using static OrchidMod.Effects.Primitives;

namespace OrchidMod.Effects.Trails
{
    public class SimpleTrail : Trail
    {
        protected readonly WidthDelegate _width;
        protected readonly ColorDelegate _color;

        public SimpleTrail(int length, WidthDelegate width, ColorDelegate color, Effect effect = null) : base(length, effect)
        {
            _width = width;
            _color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_points.Count <= 1) return;
			_vertices.Clear();
			
            this.PreDraw(spriteBatch);

            float progress = 0f;
            float currentWidth = _width?.Invoke(progress) ?? 0;
            Color currentColor = (_color?.Invoke(progress) ?? Color.White) * _dissolveProgress;

            Vector2 normal = (_points[1] - _points[0]).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * currentWidth / 2f;

            this.AddVertex(_points[0] + normal, currentColor, new Vector2(progress, 0));
            this.AddVertex(_points[0] - normal, currentColor, new Vector2(progress, 1));

            for (int i = 1; i < _points.Count; i++)
            {
                progress += Vector2.Distance(_points[i], _points[i - 1]) / this.Length;

                currentWidth = _width?.Invoke(progress) ?? 0;
                currentColor = (_color?.Invoke(progress) ?? Color.White) * _dissolveProgress;
                normal = (_points[i] - _points[i - 1]).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * currentWidth / 2f;

                this.AddVertex(_points[i] + normal, currentColor, new Vector2(progress, 0));
                this.AddVertex(_points[i] - normal, currentColor, new Vector2(progress, 1));
            }

            _effect.Parameters["transformMatrix"].SetValue(Primitives.GetTransformMatrix());

            var graphics = Main.instance.GraphicsDevice;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, _vertices.ToArray(), 0, (_points.Count - 1) * 2);
            }

            this.PostDraw(spriteBatch);
        }

        protected virtual void PreDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }

        public delegate float WidthDelegate(float progress);
        public delegate Color ColorDelegate(float progress);
    }
}
