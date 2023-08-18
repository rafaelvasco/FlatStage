using FlatStage.ContentPipeline;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial04 : Game
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

        canvas.DrawText(_font, "Hello World!", new Vec2(300, 300), Color.White);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    protected override void Update(float dt)
    {
    }
}