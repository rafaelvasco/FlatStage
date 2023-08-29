using FlatStage.ContentPipeline;
using FlatStage.Graphics;
using FlatStage.Input;
using System.Collections.Generic;
using System.Text;

namespace FlatStage.Tutorials;
public class Tutorials : Game
{
    private TextureFont _font = null!;
    private readonly StringBuilder _currentTutorialLabel = new();
    private List<BaseTutorial> _tutorials = new();
    private BaseTutorial _currentTutorial = null!;
    private int _index = 0;

    public Tutorials()
    {
        _tutorials.Add(new Tutorial01("Hello World"));
        _tutorials.Add(new Tutorial02("Bouncy Ball"));
        _tutorials.Add(new Tutorial03("Effects and Songs"));
        _tutorials.Add(new Tutorial04("Fonts"));

        _currentTutorial = _tutorials[_index];
    }

    protected override void Preload()
    {
        _font = Content.Get<TextureFont>("monogram");

        foreach (var tutorial in _tutorials)
        {
            tutorial.Load();
        }
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        _currentTutorial.Draw(canvas, dt);

        _currentTutorialLabel.Clear();
        _currentTutorialLabel.Append("Current: ");
        _currentTutorialLabel.Append(_currentTutorial.Name);

        canvas.DrawText(_font, _currentTutorialLabel, new Vec2(50, Stage.WindowSize.Height - 50), Color.Cyan);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
        _currentTutorial.FixedUpdate(dt);
    }

    protected override void Update(float dt)
    {
        _currentTutorial.Update(dt);

        if (Control.Keyboard.KeyPressed(Key.Tab))
        {
            _index = Calc.Wrap(_index + 1, 0, _tutorials.Count - 1);

            _currentTutorial = _tutorials[_index];
        }
    }
}
