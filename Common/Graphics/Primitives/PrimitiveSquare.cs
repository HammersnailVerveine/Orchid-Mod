using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace OrchidMod.Common.Graphics.Primitives
{
    public class PrimitiveSquare : IndexedPrimitiveData
    {
        public Vector2 Position;
        public Vector2 Size;
        public float Rotation;
        public Color Color;
        public SpriteEffects SpriteEffect;

        public PrimitiveSquare(Vector2 position, Vector2 size, float rotation, Color color, SpriteEffects spriteEffect, Effect effect) : base(PrimitiveType.TriangleStrip, 0, null, null, effect)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
            Color = color;
            SpriteEffect = spriteEffect;

            Indeces.AddRange(new short[] { 0, 1, 2, 3 });
            PrimitivesCount = Indeces.Count - 2;
        }

        public override void RecreateVertices()
        {
            Vertices.Clear();

            void AddVertex(Vector2 offset)
            {
                var texCoord = new Vector2((Convert.ToBoolean(offset.X) ^ SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally)).ToInt(), (Convert.ToBoolean(offset.Y) ^ SpriteEffect.HasFlag(SpriteEffects.FlipVertically)).ToInt());
                Vertices.Add(new(new Vector3(Position - Main.screenPosition + (Vector2.Subtract(offset, new Vector2(0.5f)) * Size).RotatedBy(Rotation), 0), Color, texCoord));
            }

            AddVertex(new(0, 1));
            AddVertex(new(0, 0));
            AddVertex(new(1, 1));
            AddVertex(new(1, 0));
        }
    }
}