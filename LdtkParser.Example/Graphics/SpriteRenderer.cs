using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Graphics
{
    class SpriteRenderer
    {
        private SpriteBatch spriteBatch;
        private float scale;

        public SpriteRenderer(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        public void SetScale(float s)
        {
            scale = s;
        }

        public void Begin()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }

        public void DrawAt(Texture2D texture, Rectangle sprite, Vector2 destination)
        {
            var scaledDestination = Vector2.Multiply(destination, scale);

            spriteBatch.Draw(texture, scaledDestination, sprite, Color.White, 0f, new Vector2(0f), scale, SpriteEffects.None, 0f);
        }

        public void Commit()
        {
            spriteBatch.End();
        }

    }
}
