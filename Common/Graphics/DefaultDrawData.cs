using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Common.Graphics
{
	public struct DefaultDrawData : IDrawData
	{
		public readonly bool UseDestinationRectangle;

		public Texture2D Texture;
		public Vector2 Position;
		public Rectangle DestinationRectangle;
		public Rectangle? SourceRect;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public Vector2 Scale;
		public SpriteEffects Effect;

		public DefaultDrawData(Texture2D texture, Vector2 position, Color color)
		{
			Texture = texture;
			Position = position;
			Color = color;
			DestinationRectangle = default;
			SourceRect = null;
			Rotation = 0f;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			UseDestinationRectangle = false;
		}

		public DefaultDrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color)
		{
			Texture = texture;
			Position = position;
			Color = color;
			DestinationRectangle = default;
			SourceRect = sourceRect;
			Rotation = 0f;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			UseDestinationRectangle = false;
		}

		public DefaultDrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
		{
			Texture = texture;
			Position = position;
			SourceRect = sourceRect;
			Color = color;
			Rotation = rotation;
			Origin = origin;
			Scale = new Vector2(scale, scale);
			Effect = effect;
			DestinationRectangle = default;
			UseDestinationRectangle = false;
		}

		public DefaultDrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect)
		{
			Texture = texture;
			Position = position;
			SourceRect = sourceRect;
			Color = color;
			Rotation = rotation;
			Origin = origin;
			Scale = scale;
			Effect = effect;
			DestinationRectangle = default;
			UseDestinationRectangle = false;
		}

		public DefaultDrawData(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			Texture = texture;
			DestinationRectangle = destinationRectangle;
			Color = color;
			Position = Vector2.Zero;
			SourceRect = null;
			Rotation = 0f;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			UseDestinationRectangle = true;
		}

		public DefaultDrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color)
		{
			Texture = texture;
			DestinationRectangle = destinationRectangle;
			Color = color;
			Position = Vector2.Zero;
			SourceRect = sourceRect;
			Rotation = 0f;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			UseDestinationRectangle = true;
		}

		public DefaultDrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, SpriteEffects effect)
		{
			Texture = texture;
			DestinationRectangle = destinationRectangle;
			SourceRect = sourceRect;
			Color = color;
			Rotation = rotation;
			Origin = origin;
			Effect = effect;
			Position = Vector2.Zero;
			Scale = Vector2.One;
			UseDestinationRectangle = true;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (UseDestinationRectangle)
			{
				spriteBatch.Draw(Texture, DestinationRectangle, SourceRect, Color, Rotation, Origin, Effect, 0f);
			}
			else
			{
				spriteBatch.Draw(Texture, Position, SourceRect, Color, Rotation, Origin, Scale, Effect, 0f);
			}
		}
	}
}