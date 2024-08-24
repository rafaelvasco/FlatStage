namespace FlatStage.Demos;

public abstract class BaseTutorial(string name)
{
    public string Name { get; set; } = name;

    public abstract void Load();

    public virtual void FixedUpdate(float dt) { }

    public abstract void Update(float dt);

    public abstract void Draw(Canvas canvas);
}
