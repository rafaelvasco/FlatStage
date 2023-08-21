using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial01 : Scene
{
    private Texture? _texture = null!;

    protected override void Preload()
    {
        _texture = Content.Get<Texture>("stagelogo2", embeddedAsset: true);

        GraphicsContext.SetViewClear(0, Color.CornflowerBlue);
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        canvas.Draw(_texture!, new RectF(0, 0, 128, 128), null, Color.White * 0.1f);

        canvas.Draw(_texture!, new RectF(128, 128, 128, 128), null, Color.White);

        canvas.Draw(_texture!, new RectF(256, 256, 128, 128), null, Color.White * 0.1f);

        canvas.Draw(_texture!, new RectF(384, 384, 128, 128), null, Color.White);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    protected override void Update(float dt)
    {
    }
}