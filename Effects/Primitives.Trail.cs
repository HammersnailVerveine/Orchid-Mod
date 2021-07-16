using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Effects
{
    public partial class Primitives
    {
        public abstract class Trail
        {
            public bool Active { get; set; } = true;
            public Entity Target { protected get; set; }
            public float Length { get; protected set; }
            public int MaxPoints { get; set; } = 25;

            protected readonly int _maxLength;
            protected readonly Effect _effect;
            protected readonly List<Vector2> _points = new List<Vector2>();
            protected readonly List<VertexPositionColorTexture> _vertices = new List<VertexPositionColorTexture>();

            protected bool _dissolving = false;
            protected float _dissolveSpeed = 0.1f;
            protected float _dissolveProgress = 1f;

            public Trail(int length, Effect effect = null)
            {
                _maxLength = length;
                _effect = effect ?? OrchidMod.Instance.GetEffect("Effects/Primitive");
                _effect.Parameters["texture0"].SetValue(ModContent.GetTexture("OrchidMod/Effects/Textures/Pixel"));
            }

            public void Update()
            {
                if (this.Target == null || !this.Target.active || !Active)
                {
                    _dissolving = true;
                    this.Target = null;
                }

                int length = _maxLength;

                if (_dissolving)
                {
                    _dissolveProgress -= _dissolveSpeed;
                    length = (int)(length * _dissolveProgress);
                    if (_dissolveProgress <= 0) this.Kill();
                }

                if (this.PreUpdate())
                {
                    this.UpdatePoints(maxLength: length);
                }
                this.PostUpdate();

                this.UpdateEffectParameters();
            }

            public void Kill()
            {
                this.Active = false;
                this.PreKill();
                OrchidMod.Primitives?._trails.Remove(this);
            }

            public void SetEffectTexture(Texture2D texture, int index = 0) => _effect.Parameters["texture" + index].SetValue(texture);
            public void SetDissolveSpeed(float speed = 0.1f) => _dissolveSpeed = speed;
			public void StartDissolving() => _dissolving = true;

            protected void UpdatePoints(int maxLength)
            {
                this.Length = 0;
                if (!_dissolving) _points.Insert(0, Target.Center);

                if (_points.Count <= 1) return;
                if (_points.Count > this.MaxPoints) _points.Remove(_points.Last());

                int lastIndex = -1;
                for (int i = 1; i < _points.Count; i++)
                {
                    float dist = Vector2.Distance(_points[i], _points[i - 1]);

                    this.Length += dist;
                    if (this.Length > maxLength)
                    {
                        lastIndex = i;
                        this.Length -= dist;
                        break;
                    }
                }
                if (lastIndex < 0) return;

                var vector = Vector2.Normalize(_points[lastIndex] - _points[lastIndex - 1]) * (maxLength - this.Length);
                _points.RemoveRange(lastIndex, _points.Count() - lastIndex);
                _points.Add(_points[lastIndex - 1] + vector);
                this.Length = maxLength;
            }

            protected void UpdateEffectParameters()
            {
                foreach (var param in _effect.Parameters)
                {
                    if (param.Name == "time") _effect.Parameters["time"].SetValue(Main.GlobalTime);
                }
            }

            protected void AddVertex(Vector2 position, Color color, Vector2 uv)
            {
                Vector3 pos = new Vector3(position.X - Main.screenPosition.X, position.Y - Main.screenPosition.Y, 0);
                _vertices.Add(new VertexPositionColorTexture(pos, color, uv));
            }

            public virtual void Draw(SpriteBatch spriteBatch) { }
            protected virtual void PostUpdate() { }
            protected virtual void PreKill() { }
            protected virtual bool PreUpdate() { return true; }
        }
    }
}
