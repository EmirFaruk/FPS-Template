using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndSreenPresenter
{
    private VisualElement root;

    public EndSreenPresenter(VisualElement root)
    {
        this.root = root;
        PlayerHealth.OnDeath += () => GetActive();
        root.Q<Button>("Button_MainMenu").clicked += GoToMainMenu;
    }

    private void GetActive()
    {
        root.SetEnability(null, true);
        root.Pause(true);
        root.SetCursorVisibility(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //WidgetData.Widgets.MainMenu.OpenWidget(true);
    }
}