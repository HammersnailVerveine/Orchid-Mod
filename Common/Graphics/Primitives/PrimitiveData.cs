using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace OrchidMod.Common.Graphics.Primitives
{
    public class PrimitiveData : IDrawData
    {
		public static readonly IPrimitiveEffect.Default NullEffect = new();
		public static readonly IPrimitiveTip.Without NullTip = new();

		// ...

		public IPrimitiveEffect PrimitiveEffect { get; private set; }
        public PrimitiveType PrimitiveType { get; private set; }

        // ...

        public List<VertexPositionColorTexture> Vertices;
        public int PrimitivesCount;

        // ...

        public PrimitiveData(PrimitiveType type, int primitivesCount, List<VertexPositionColorTexture> vertices, IPrimitiveEffect effect)
        {
			PrimitiveEffect = effect;
            PrimitiveType = type;
            PrimitivesCount = primitivesCount;
            Vertices = vertices ?? new List<VertexPositionColorTexture>();
			PrimitiveEffect = effect ?? NullEffect;
        }

        public void UpdateEffectParameters()
		{
			PrimitiveEffect.Effect.Value.Parameters["WorldViewProj"].SetValue(PrimitiveEffect.Matrix);
			PrimitiveEffect.SetParameters(PrimitiveEffect.Effect.Value.Parameters);
		}

		public void Draw(SpriteBatch spriteBatch)
        {
            RecreateVertices();

            if (PrimitivesCount <= 0) return;

			UpdateEffectParameters();
            DrawPrimitives(spriteBatch.GraphicsDevice);
        }

        public virtual void RecreateVertices() { }

        public virtual void DrawPrimitives(GraphicsDevice graphics)
        {
            foreach (var pass in PrimitiveEffect.Effect.Value.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType, Vertices.ToArray(), 0, PrimitivesCount);
            }
        }
	}

    public class IndexedPrimitiveData : PrimitiveData
    {

        public List<short> Indeces;

        // ...

        public IndexedPrimitiveData(PrimitiveType type, int primitivesCount, List<VertexPositionColorTexture> vertices, List<short> indeces, IPrimitiveEffect effect) : base(type, primitivesCount, vertices, effect)
        {
            Indeces = indeces ?? new List<short>();
        }

        public override void DrawPrimitives(GraphicsDevice graphics)
        {
            foreach (var pass in PrimitiveEffect.Effect.Value.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType, Vertices.ToArray(), 0, Vertices.Count, Indeces.ToArray(), 0, PrimitivesCount);
            }
        }
    }
}
