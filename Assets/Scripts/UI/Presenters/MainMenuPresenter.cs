using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter : VisualElement
{
    // Events
    public static event Action OnStartButtonClicked = () => { };
    public static event Action OnOptionsButtonClicked = () => { };

    private VisualElement root;

    // Buttons
    private Button startButton;
    private Button quitButton;

    public MainMenuPresenter(VisualElement root)
    {
        this.root = root;
        //root.Q<Button>
        startButton = root.Q<Button>("Button_Start");
        startButton.RegisterCallback<ClickEvent>(StartGame);

        Button optionsButton = root.Q<Button>("Button_Options");
        optionsButton.clicked += () => OnOptionsButtonClicked?.Invoke();

        quitButton = root.Q<Button>("Button_Quit");
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    #region Buttons

    void StartGame(ClickEvent evt)
    {
        Debug.Log("Game Started!");

        startButton.text = "Resume";
        root.SetEnability(false);

        OnStartButtonClicked.Invoke();
    }

    void QuitGame(ClickEvent evt)
    {
        Debug.Log("Game Quit!");
        Application.Quit();

        #region Editor : Quit
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        #endregion
    }

    #endregion
}
