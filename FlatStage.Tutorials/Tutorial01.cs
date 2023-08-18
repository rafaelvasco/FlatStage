using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial01 : Game
{
    private Texture? _texture;

    protected override void Preload()
    {
        _texture = Content.Get<Texture>("stagelogo2", embeddedAsset: true);

        GraphicsContext.SetViewClear(0, Color.Black);
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        canvas.Draw(_texture!, GraphicsContext.BackbufferWidth / 2f, GraphicsContext.BackbufferHeight / 2f, new Vec2(0.5f, 0.5f),
            Color.White);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    protected override void Update(float dt)
    {
    }
}