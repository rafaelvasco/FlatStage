namespace FlatStage;

public class MyGame : Game
{
    protected override void Update(float dt)
    {
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        var textSize = BuiltinContent.Fonts.ChikareGo2.MeasureString("Hello World!\nHello Again!", 4);
        var textPos = new Vec2(Canvas.Width / 2f - textSize.X / 2f, Canvas.Height / 2f);

        canvas.Draw(BuiltinContent.Textures.Logo, new Vec2(Canvas.Width/2f - BuiltinContent.Textures
            .Logo.Width/2f, 10), Color.White);
        canvas.DrawText(BuiltinContent.Fonts.ChikareGo2, "Hello World!\nHello Again!", textPos, new Vec2(4,4), Color.White);
        canvas.DrawText(BuiltinContent.Fonts.ChikareGo2, "Hello World!\nHello Again!", new Vec2(0,textPos.Y + textSize.Y), new Vec2(1, 1), Color.DeepPink);
    }
}
