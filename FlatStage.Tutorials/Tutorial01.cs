using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial01 : BaseTutorial
{
    private Texture? _texture = null!;

    public Tutorial01(string name) : base(name)
    {
    }

    public override void Load()
    {
        _texture = Content.Get<Texture>("stagelogo", embeddedAsset: true);

        GraphicsContext.SetViewClear(0, Color.CornflowerBlue);
    }

    public override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Draw(_texture!, new Vec2(Stage.WindowSize.Width / 2f, Stage.WindowSize.Height / 2f), new Vec2(0.5f, 0.5f), Color.White);
    }
}