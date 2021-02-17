using Game1.Graphics;
using Game1.LdtkParser;
using Game1.LdtkParser.Graphics;
using Game1.LdtkParser.Layers;

namespace Game1.Entity
{
    class Player : RenderableEntity, ILdtkEntity
    {
        public enum KeyFrame
        {
            Idle,
            Walking1,
            Walking2,
            Up,
            Down,
            SuperJump1,
            SuperJump2,
            SuperJump3,
            Death1,
            Death2,
            Death3,
            Death4,
            Death5,
            Death6
        };

        public KeyFrame currentFrame = KeyFrame.Idle;
        public SpriteEnum Animations;
        public void FromLdtk(EntityModel entity)
        {

        }

        public void Update()
        {

        }

        public override void Draw()
        {
            spriteRenderer.DrawAt(Animations.Tileset.Texture, Animations.GetFrameByKey(currentFrame.ToString()).Source, Pos);
        }
    }
}
