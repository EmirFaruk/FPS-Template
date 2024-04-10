using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter : VisualElement
{
    #region VARIABLES

    // Events
    public static event Action OnStartButtonClicked = () => { };
    public static event Action OnResumeButtonClicked = () => { };
    public static event Action OnOptionsButtonClicked = () => { };

    private VisualElement root;

    // Buttons
    private Button startButton;
    private Button quitButton;

    private const string BUTTON_START = "Button_Start";
    private const string BUTTON_OPTIONS = "Button_Options";
    private const string BUTTON_QUIT = "Button_Quit";
    private const string RESUME = "Resume";

    #endregion

    public MainMenuPresenter(VisualElement root)
    {
        this.root = root;
        //root.Q<Button>
        startButton = root.Q<Button>(BUTTON_START);
        startButton.RegisterCallback<ClickEvent>(StartGame);

        Button optionsButton = root.Q<Button>(BUTTON_OPTIONS);
        optionsButton.clicked += () => OnOptionsButtonClicked?.Invoke();

        quitButton = root.Q<Button>(BUTTON_QUIT);
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    #region Buttons

    void StartGame(ClickEvent evt)
    {
        Debug.Log("Game Started!");

        if (startButton.text != RESUME) OnStartButtonClicked.Invoke();
        else OnResumeButtonClicked.Invoke();

        startButton.text = RESUME;

        root.SetEnability(false);

    }

    void QuitGame(ClickEvent evt)
    {
        Debug.Log("Quit!");
        Application.Quit();

        #region Editor : Quit
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        #endregion
    }

    #endregion
}
