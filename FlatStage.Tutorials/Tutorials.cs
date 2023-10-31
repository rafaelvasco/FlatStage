using FlatStage.Content;
using FlatStage.Graphics;
using FlatStage.Input;
using System.Collections.Generic;

namespace FlatStage.Tutorials;
public class Tutorials : Game
{
    private readonly StringBuffer _currentTutorialLabel = new();
    private List<BaseTutorial> _tutorials = new();
    private BaseTutorial _currentTutorial = null!;
    private int _index = 0;

    public Tutorials()
    {
        Settings = new GameSettings()
        {
            AppTitle = "FlatStage Core Tutorials",
            CanvasWidth = 800,
            CanvasHeight = 600,
        };

        _tutorials.Add(new Tutorial01("Hello World"));
        _tutorials.Add(new Tutorial02("Bouncy Ball"));
        _tutorials.Add(new Tutorial03("Effects and Songs"));
        _tutorials.Add(new Tutorial04("Fonts"));
        _tutorials.Add(new Tutorial05("Gui"));

        _currentTutorial = _tutorials[_index];
    }

    protected override void Load()
    {
        foreach (var tutorial in _tutorials)
        {
            tutorial.Load();
        }
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        _currentTutorial.Draw(canvas);

        _currentTutorialLabel.Clear();
        _currentTutorialLabel.Append("Current: ");
        _currentTutorialLabel.Append(_currentTutorial.Name);

        canvas.DrawText(BuiltinContent.Fonts.Monogram, _currentTutorialLabel.ReadOnlySpan, new Vec2(50, Canvas.Height - 50), Color.Cyan);
    }

    protected override void FixedUpdate(float dt)
    {
        _currentTutorial.FixedUpdate(dt);
    }

    protected override void Update(float dt)
    {
        _currentTutorial.Update(dt);

        if (Keyboard.KeyPressed(Key.Tab))
        {
            _index++;

            if (_index > _tutorials.Count - 1)
            {
                _index = 0;
            }

            _currentTutorial = _tutorials[_index];
        }
    }
}
