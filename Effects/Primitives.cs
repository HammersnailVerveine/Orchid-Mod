using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.IO;

namespace OrchidMod.Effects
{
    public partial class Primitives
    {
        private readonly List<Trail> _trails = new List<Trail>();

        public int TrailCount => _trails.Count();

        public void CreateTrail(Entity target, Trail trail)
        {
            if (Main.dedServ) return;

            trail.Target = target;
            _trails.Add(trail);
        }

        public void DrawTrails(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (var trail in _trails.FindAll(i => i.Active)) trail.Draw(spriteBatch);
            spriteBatch.End();
        }

		public void UpdateTrails()
        {
            foreach (var trail in _trails.ToList()) trail.Update();
        }

        public static Matrix GetTransformMatrix()
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Main.screenWidth / 2, -Main.screenHeight / 2, 0) * Matrix.CreateRotationZ(MathHelper.Pi);
            Matrix projection = Matrix.CreateOrthographic(Main.screenWidth, Main.screenHeight, 0, 1000);
            return view * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1) * projection;
        }
    }
}
