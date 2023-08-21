using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial04 : Scene
{
    private TextureFont _font = null!;

    protected override void Preload()
    {
        _font = Content.Get<TextureFont>("monogram");

        GraphicsContext.SetViewClear(0, Color.Black);
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        canvas.DrawText(_font, "Flat Stage Engine!", new Vec2(0, 0), Color.White);
        canvas.DrawText(_font, "Flat Stage Engine!", new Vec2(50, 50), new Vec2(2.0f, 2.0f), Color.White);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    protected override void Update(float dt)
    {
    }
}