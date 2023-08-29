using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial04 : BaseTutorial
{
    private TextureFont _font = null!;

    public Tutorial04(string name) : base(name)
    {
    }

    public override void Load()
    {
        _font = Content.Get<TextureFont>("monogram");

        GraphicsContext.SetViewClear(0, Color.Black);
    }

    public override void Draw(Canvas2D canvas, float dt)
    {
        canvas.DrawText(_font, "Flat Stage Engine!", new Vec2(0, 0), Color.White);
        canvas.DrawText(_font, "Flat Stage Engine!", new Vec2(50, 50), new Vec2(2.0f, 2.0f), Color.White);
    }
}