using FlatStage.Graphics;

namespace FlatStage.Tutorials;
public abstract class BaseTutorial
{
    public string Name { get; set; }

    protected BaseTutorial(string name)
    {
        Name = name;
    }

    public abstract void Load();

    public virtual void FixedUpdate(float dt) { }

    public virtual void Update(float dt) { }

    public abstract void Draw(Canvas2D canvas, float dt);
}
