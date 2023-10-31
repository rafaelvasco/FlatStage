using FlatStage.Content;
using FlatStage.Graphics;

namespace FlatStage.Tutorials;

public class Tutorial04 : BaseTutorial
{
    public Tutorial04(string name) : base(name)
    {
    }

    public override void Load()
    {
    }

    public override void Draw(Canvas canvas)
    {
        canvas.DrawText(BuiltinContent.Fonts.Monogram, "Flat Stage Engine!", new Vec2(0, 0), Color.White);
        canvas.DrawText(BuiltinContent.Fonts.Monogram, "Flat Stage Engine!", new Vec2(50, 50), new Vec2(2.0f, 2.0f), Color.White);
    }

    public override void Update(float dt)
    {
    }
}
