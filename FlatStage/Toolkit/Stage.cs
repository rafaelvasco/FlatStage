using FlatStage.Content;
using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class Stage : Game
{
    protected Gui? Gui => _gui;

    private readonly GameDef _gameDefinition;

    public Stage()
    {
        _gameDefinition = LoadFromDefinition();

        Settings = _gameDefinition.GameSettings;
    }

    public void GoToScene(string sceneName)
    {
        if (_scenesMap.TryGetValue(sceneName, out var gameObject))
        {
            _currentScene = gameObject;
        }
    }

    protected override void Load()
    {
        Canvas.StretchMode = Settings.CanvasStretchMode;

        if (!string.IsNullOrEmpty(_gameDefinition.PreloadPak))
        {
            Assets.LoadPak(_gameDefinition.PreloadPak);
        }

        PopulateGame(_gameDefinition);

        OnReady();
    }

    protected virtual void OnReady() { }

    protected override void Update(float dt)
    {
        _currentScene.Update(dt);
    }

    protected override void FixedUpdate(float dt)
    {
        _gui?.Update(dt);
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        _currentScene.Draw(canvas);
        _gui?.Draw(canvas);
    }

    private static GameDef LoadFromDefinition()
    {
        var gameDef = Assets.LoadDefinitionData<GameDef>(ContentProperties.StageProjectFile);

        IDefinitionData.ThrowIfInValid(gameDef, "Stage::LoadFromDefinition");

        return gameDef;
    }

    private void PopulateGame(GameDef gameDef)
    {
        _scenes = new GameEntity[gameDef.Scenes.Length];

        for (int i = 0; i < gameDef.Scenes.Length; ++i)
        {
            var scene = _scenes[i] = GameEntity.FromDefinition(this, gameDef.Scenes[i], null);
            scene.Width = Canvas.Width;
            scene.Height = Canvas.Height;
            _scenesMap[scene.Name] = scene;
        }

        if (gameDef.Gui != null)
        {
            _gui = new Gui();
            _gui.InitFromDefinition(gameDef.Gui);
        }

        _currentScene = _scenesMap[gameDef.StartingScene];
    }

    internal void Register(GameEntity gameObject)
    {
        _gameObjectsMap[gameObject.Name] = gameObject;
    }

    private GameEntity _currentScene = null!;

    private GameEntity[] _scenes = null!;

    private Gui? _gui;
    private readonly FastDictionary<string, GameEntity> _scenesMap = new();
    private readonly FastDictionary<string, GameEntity> _gameObjectsMap = new();

}
