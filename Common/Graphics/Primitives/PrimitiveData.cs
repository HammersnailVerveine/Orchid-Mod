using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Common.Graphics.Primitives
{
    public class PrimitiveData : IDrawData
    {
        public Effect Effect { get; private set; }
        public PrimitiveType PrimitiveType { get; private set; }

        // ...

        public List<VertexPositionColorTexture> Vertices;
        public int PrimitivesCount;

        // ...

        public delegate void OnUpdateEffectParametersDelegate(Effect effect);
        public OnUpdateEffectParametersDelegate OnUpdateEffectParameters;

        // ...

        public PrimitiveData(PrimitiveType type, int primitivesCount, List<VertexPositionColorTexture> vertices, Effect effect)
        {
            Effect = effect;
            PrimitiveType = type;
            PrimitivesCount = primitivesCount;
            Vertices = vertices ?? new List<VertexPositionColorTexture>();
            Effect = effect;
			OnUpdateEffectParameters = (_) => { };
        }

        public void UpdateEffectParameters(Matrix matrix)
        {
            Effect.Parameters["WorldViewProj"].SetValue(matrix);
            OnUpdateEffectParameters.Invoke(Effect);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RecreateVertices();

            if (PrimitivesCount <= 0) return;

			var matrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up);
			matrix *= Main.GameViewMatrix.EffectMatrix;
			matrix *= Matrix.CreateTranslation(Main.screenWidth / 2, Main.screenHeight / -2, 0);
			matrix *= Matrix.CreateRotationZ(MathHelper.Pi);
			matrix *= Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1);
			matrix *= Matrix.CreateOrthographic(Main.screenWidth, Main.screenHeight, 0, 1000);

			UpdateEffectParameters(matrix);
            DrawPrimitives(spriteBatch.GraphicsDevice);
        }

        public virtual void RecreateVertices() { }

        public virtual void DrawPrimitives(GraphicsDevice graphics)
        {
            foreach (var pass in Effect.CurrentTechnique.Passes)
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

        public IndexedPrimitiveData(PrimitiveType type, int primitivesCount, List<VertexPositionColorTexture> vertices, List<short> indeces, Effect effect) : base(type, primitivesCount, vertices, effect)
        {
            Indeces = indeces ?? new List<short>();
        }

        public override void DrawPrimitives(GraphicsDevice graphics)
        {
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType, Vertices.ToArray(), 0, Vertices.Count, Indeces.ToArray(), 0, PrimitivesCount);
            }
        }
    }
}
