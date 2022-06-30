using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Common.Graphics.Primitives
{
	public class PrimitiveStrip : IndexedPrimitiveData
	{
		public IPrimitiveTip HeadTip { get; private set; }
		public IPrimitiveTip TailTip { get; private set; }

		public List<Vector2> Points;

		// ...

		public delegate float WidthFunctionDelegate(float progress);
		public WidthFunctionDelegate WidthFunction;

		public delegate Color ColorFunctionDelegate(float progress);
		public ColorFunctionDelegate ColorFunction;

		// ...

		public PrimitiveStrip(WidthFunctionDelegate width, ColorFunctionDelegate color, IPrimitiveEffect effect, IPrimitiveTip headTip = null, IPrimitiveTip tailTip = null) : base(PrimitiveType.TriangleList, 0, null, null, effect)
		{
			WidthFunction += width;
			ColorFunction += color;
			Points = new List<Vector2>();

			HeadTip = headTip ?? NullTip;
			TailTip = tailTip ?? NullTip;
		}

		public override void RecreateVertices()
		{
			Vertices.Clear();
			Indeces.Clear();

			if (Points.Count < 2) return;

			var length = 0f;
			var distances = new float[Points.Count - 1];

			for (int i = 1; i < Points.Count; i++)
			{
				var j = i - 1;
				distances[j] = Vector2.DistanceSquared(Points[j], Points[i]);
				length += distances[j];
			}

			var progress = 0f;
			(Color color, Vector2 normal) = GetProgressVariables(0);

			HeadTip.CreateTipMesh(ref Vertices, ref Indeces, Points[0], normal, color, 0);

			Vertices.AddVertex(Points[0] - normal, color, new Vector2(progress, 0));
			Vertices.AddVertex(Points[0] + normal, color, new Vector2(progress, 1));

			var nextIndex = Indeces.Count;

			for (int i = 1; i < Points.Count; i++)
			{
				progress += distances[i - 1] / length;
				(color, normal) = GetProgressVariables(progress, i);

				Vertices.AddVertex(Points[i] - normal, color, new Vector2(progress, 0));
				Vertices.AddVertex(Points[i] + normal, color, new Vector2(progress, 1));

				var i2 = nextIndex + i * 2 - 2;

				Indeces.AddRange(new short[]
				{
					(short)i2,
					(short)(i2 + 2),
					(short)(i2 + 1),
					(short)(i2 + 2),
					(short)(i2 + 3),
					(short)(i2 + 1)
				});
			}

			TailTip.CreateTipMesh(ref Vertices, ref Indeces, Points[^1], -normal, color, 1);

			PrimitivesCount = Points.Count * 2 - 2 + (int)HeadTip.ExtraTriangles + (int)TailTip.ExtraTriangles;
		}

		private (Color, Vector2) GetProgressVariables(float progress, int index = 1)
		{
			var width = WidthFunction?.Invoke(progress) ?? 0f;
			var color = ColorFunction?.Invoke(progress) ?? Color.Transparent;
			var normal = (Points[index] - Points[index - 1]).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width / 2f;

			return (color, normal);
		}
	}
}