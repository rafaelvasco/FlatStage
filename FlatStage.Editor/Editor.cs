using FlatStage.Toolkit;

namespace FlatStage.Editor;

public class Editor : Stage
{
    protected override void OnReady()
    {
        if (Gui != null)
        {
            var menuBar = Gui.Get<GuiMenuBar>("menuBar");

            menuBar.SetEventHandler("newProject", OnNewProjectClicked);
            menuBar.SetEventHandler("quit", OnQuitClicked);
            menuBar.SetEventHandler("about", OnAboutClicked);
        }
    }

    private void OnNewProjectClicked(GuiMenuItem menuItem)
    {
        NewProject();
    }

    private void OnAboutClicked(GuiMenuItem menuItem)
    {
        Gui?.Open("aboutWindow");
    }

    private void OnQuitClicked(GuiMenuItem item)
    {
        Exit();
    }

    private void NewProject()
    {
        _currentProject = new GameDef()
        {
            GameSettings = new GameSettings()
            {
                AppTitle = "New Project",
                CanvasWidth = 800,
                CanvasHeight = 600,
            },
            Scenes = new GameObjectDef[]
            {
                new GameObjectDef()
                {
                    Name = "Scene1",
                }
            },
            StartingScene = "Scene1"
        };
    }

    private GameDef? _currentProject;
}
