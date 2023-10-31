using FlatStage.Graphics;

namespace FlatStage.Tutorials;
public abstract class BaseTutorial
{
    public string Name { get; set; }

    public abstract void Load();

    public virtual void FixedUpdate(float dt) { }

    public abstract void Update(float dt);

    public abstract void Draw(Canvas canvas);

    protected BaseTutorial(string name)
    {
        Name = name;
    }
}
