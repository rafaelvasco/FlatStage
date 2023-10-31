namespace FlatStage.Toolkit.Tutorials;
public class Tutorials : Stage
{
    protected override void OnReady()
    {
        if (Gui != null)
        {
            var menuBar = Gui.Get<GuiMenuBar>("menuBar");

            menuBar.SetEventHandler("tutorial01", OnTutorial01Clicked);
            menuBar.SetEventHandler("tutorial02", OnTutorial02Clicked);
        }
    }

    private void OnTutorial01Clicked(GuiMenuItem _)
    {
        GoToScene("Tutorial01");
    }

    private void OnTutorial02Clicked(GuiMenuItem _)
    {
        GoToScene("Tutorial02");
    }
}
