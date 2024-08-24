namespace FlatStage.Demos;

public class Tutorial01(string name) : BaseTutorial(name)
{
    private Texture _texture = null!;

    private int _x, _y;

    public override void Load()
    {
        _texture = BuiltinContent.Textures.Logo;
    }

    public override void Update(float dt)
    {
        if (Keyboard.KeyDown(Key.Left))
        {
            _x -= 4;
        }

        if (Keyboard.KeyDown(Key.Right))
        {
            _x += 4;
        }

        if (Keyboard.KeyDown(Key.Up))
        {
            _y -= 4;
        }

        if (Keyboard.KeyDown(Key.Down))
        {
            _y += 4;
        }

        Canvas.SetViewRegion(new Rect(_x, _y, Canvas.Width, Canvas.Height));
    }

    public override void Draw(Canvas canvas)
    {
        canvas.Draw(_texture, new Vec2(Canvas.Width / 2f, Canvas.Height / 2f), new Vec2(0.5f, 0.5f), Color.White);
    }
}
