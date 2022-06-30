using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Common.Graphics.Primitives
{
	public interface IPrimitiveTip
	{
		abstract uint ExtraTriangles { get; }

		void CreateTipMesh(ref List<VertexPositionColorTexture> vertices, ref List<short> indeces, Vector2 position, Vector2 normal, Color color, int texCoordY);

		// ...

		public struct Without : IPrimitiveTip
		{
			uint IPrimitiveTip.ExtraTriangles => 0;

			void IPrimitiveTip.CreateTipMesh(ref List<VertexPositionColorTexture> vertices, ref List<short> indeces, Vector2 position, Vector2 normal, Color color, int texCoordY) { }
		}

		public struct Triangular : IPrimitiveTip
		{
			public readonly float? TipLength;

			public Triangular()
			{
				TipLength = null;
			}

			public Triangular(float tipLength)
			{
				TipLength = tipLength;
			}

			uint IPrimitiveTip.ExtraTriangles => 1;

			void IPrimitiveTip.CreateTipMesh(ref List<VertexPositionColorTexture> vertices, ref List<short> indeces, Vector2 position, Vector2 normal, Color color, int texCoordY)
			{
				var length = TipLength ?? normal.Length();
				var nextIndex = vertices.Count;

				vertices.AddVertex(position - Vector2.Normalize(normal).RotatedBy(-MathHelper.PiOver2) * length, color, new Vector2(texCoordY, 0.5f));
				vertices.AddVertex(position - normal, color, new Vector2(texCoordY, texCoordY));
				vertices.AddVertex(position + normal, color, new Vector2(texCoordY, 1 - texCoordY));

				indeces.Add((short)nextIndex);
				indeces.Add((short)(nextIndex + 1));
				indeces.Add((short)(nextIndex + 2));
			}
		}

		public struct Rounded : IPrimitiveTip
		{
			public readonly uint Smoothness;

			public Rounded()
			{
				Smoothness = 15;
			}

			public Rounded(uint smoothness)
			{
				Smoothness = Math.Max(smoothness, 2);
			}

			uint IPrimitiveTip.ExtraTriangles => Smoothness;

			void IPrimitiveTip.CreateTipMesh(ref List<VertexPositionColorTexture> vertices, ref List<short> indeces, Vector2 position, Vector2 normal, Color color, int texCoordY)
			{
				var angle = MathHelper.Pi / Smoothness;
				var currentAngle = 0f;
				var nextIndex = vertices.Count;

				for (int i = 0; i < Smoothness; i++)
				{
					vertices.AddVertex(position, color, new Vector2(0.5f, 0.5f));
					vertices.AddVertex(position + normal.RotatedBy(currentAngle), color, new Vector2(1, 1));

					currentAngle += angle;
					var i2 = nextIndex + i * 3;

					vertices.AddVertex(position + normal.RotatedBy(currentAngle), color, new Vector2(0, 1));

					indeces.Add((short)i2);
					indeces.Add((short)(i2 + 1));
					indeces.Add((short)(i2 + 2));
				}
			}
		}
	}
}