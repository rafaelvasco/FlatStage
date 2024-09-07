namespace FlatStage;

public class MyGame : Game
{
    protected override void Update(float dt)
    {
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        const int scale = 1;

        canvas.Draw(BuiltinContent.Textures.Logo, new Vec2(Canvas.Width/2f - BuiltinContent.Textures
            .Logo.Width/2f, 10), Color.White);
        canvas.DrawText(BuiltinContent.Fonts.ChikareGo2, "Hello World!\nHello Again!", new Vec2(0, 0), new Vec2(scale, scale), Color.White);
        canvas.DrawText(BuiltinContent.Fonts.ChikareGo2, "Hello World!\nHello Again!", new Vec2(0, 50*scale), new Vec2(scale, scale), Color.DeepPink);
        canvas.DrawText(BuiltinContent.Fonts.Monogram, "Hello World!\nHello Again!", new Vec2(0, 100*scale), scale: new Vec2(scale, scale), Color.DodgerBlue);
        canvas.DrawText(BuiltinContent.Fonts.Koubit001, "Hello World!\nHello Again!", new Vec2(0, 150*scale), scale: new Vec2(scale, scale), Color.Fuchsia);
    }
}
