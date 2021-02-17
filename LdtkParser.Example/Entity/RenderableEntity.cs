using Game1.Graphics;

namespace Game1.Entity
{
    abstract class RenderableEntity : BaseEntity
    {
        protected SpriteRenderer spriteRenderer;

        public void SetSpriteRenderer(SpriteRenderer sr)
        {
            spriteRenderer = sr;
        }

        abstract public void Draw();
    }
}
