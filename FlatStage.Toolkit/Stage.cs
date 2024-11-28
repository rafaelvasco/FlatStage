namespace FlatStage.Toolkit;

public delegate void DefaultEventHandler();

public class Stage : Game
{
    public GameObject Scene { get; private set; }

    public Stage()
    {
        var defaultScene = new GameObject("default");

        Scene = defaultScene;

        _scenes.Add("default", defaultScene);

        _gameDef = LoadDefinition();
        Settings = _gameDef.GameSettings;
    }

    public void SwitchScene(string id)
    {
        if (_scenes.TryGetValue(id, out var gameObject))
        {
            Scene = gameObject;
        }
    }

    public GameObject GetScene(string sceneId)
    {
        if (_scenes.TryGetValue(sceneId, out var scene))
        {
            return scene;
        }

        FlatException.Throw($"Could not find scene with id: {sceneId}");

        return default!;
    }

    protected override void Load()
    {
        Canvas.StretchMode = Settings.CanvasStretchMode;

        Canvas.MainViewport.BackgroundColor = _gameDef.GameSettings.CanvasBackColor;

        PopulateGame(_gameDef);

        OnReady();
    }

    protected override void Update(float dt)
    {
        Scene.Update(dt);

        Behaviors.Update(dt);
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        Scene.Draw(canvas);
    }

    private static GameDef LoadDefinition()
    {
        var gameDef = DefinitionIO.LoadDefinitionData<GameDef>(ContentProperties.GameProjectFile);

        IDefinitionData.ThrowIfInValid(gameDef, "Stage::LoadDefinition [gameDef]");

        gameDef.GameObjects = new GameObjectDef[gameDef.GameObjectsPaths.Length];

        var index = 0;

        foreach (var scenePath in gameDef.GameObjectsPaths)
        {
            var sceneDef = DefinitionIO.LoadDefinitionData<GameObjectDef>($"{scenePath}.json");

            IDefinitionData.ThrowIfInValid(sceneDef, "Stage::LoadDefinition [sceneDef]");

            gameDef.GameObjects[index++] = sceneDef;
        }

        return gameDef;
    }

    private void PopulateGame(GameDef gameDef)
    {
        foreach (var sceneDef in gameDef.GameObjects)
        {
            var scene = GameObjectCreator.CreateFromDefinition(sceneDef);

            scene.Width = Canvas.Width;
            scene.Height = Canvas.Height;

            _scenes.Add(scene.Id, scene);
        }

        SwitchScene(gameDef.StartingGameObject);
    }

    protected virtual void OnReady() {}

    private readonly GameDef _gameDef;

    private readonly Dictionary<string, GameObject> _scenes = new();

}
