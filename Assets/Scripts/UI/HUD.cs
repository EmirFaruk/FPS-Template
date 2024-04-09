using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    #region VARIABLES    
    [SerializeField] private StyleSheet styleSheet;

    // Fields
    private UIDocument uIDocument;
    private VisualElement root;
    private VisualElement mainMenu;
    private VisualElement optionsMenu;
    private VisualElement gauges;
    private VisualElement endScreen;

    private List<VisualElement> widgets;
    private WidgetData widgetData;

    // Properties
    public static VisualElement ActiveElement { get; set; }
    public VisualElement PopUp { get; private set; }
    public Shader OutlineShader { get; private set; }

    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        this.Pause(true);
        this.SetCursorVisibility(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.SetCursorVisibility(!UnityEngine.Cursor.visible);
            this.Pause(UnityEngine.Cursor.visible);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isEnable = !mainMenu.IsEnable();

            this.SetCursorVisibility(isEnable);
            this.Pause(isEnable);

            mainMenu.SetEnability(ActiveElement, isEnable);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ActiveElement?.SetEnability(false);

            int Lenght = Enum.GetNames(typeof(WidgetData.Widgets)).Length;
            WidgetData.Widgets randomWidget = (WidgetData.Widgets)UnityEngine.Random.Range(0, Lenght);

            randomWidget.OpenWidget();
        }
    }

    #region Enable/Disable
    private void OnEnable()
    {
        MainMenuPresenter.OnStartButtonClicked += () => StartHUD();
        MainMenuPresenter.OnOptionsButtonClicked += () => optionsMenu.SetEnability(ActiveElement, true);
        OptionsMenuPresenter.OnBackButtonClicked += () => mainMenu.SetEnability(ActiveElement, true);
    }

    private void OnDisable()
    {
        MainMenuPresenter.OnStartButtonClicked -= () => StartHUD();
        MainMenuPresenter.OnOptionsButtonClicked -= () => optionsMenu.SetEnability(ActiveElement, true);
        OptionsMenuPresenter.OnBackButtonClicked -= () => mainMenu.SetEnability(ActiveElement, true);
    }
    #endregion

    #endregion

    #region METHODS

    private void Initialize()
    {
        uIDocument = GetComponent<UIDocument>();
        root = uIDocument.rootVisualElement;

        mainMenu = root.Q("MainMenu");
        optionsMenu = root.Q("OptionsMenu");
        gauges = root.Q("Gauges");
        endScreen = root.Q("EndScreen");
        widgets = new List<VisualElement> { mainMenu, optionsMenu, gauges, endScreen };

        widgetData = new WidgetData(widgets);

        ActiveElement = mainMenu.SetEnability(ActiveElement, true);
        optionsMenu.SetEnability(false);
        gauges.SetEnability(false);
        endScreen.SetEnability(false);

        this.Pause(true);
        this.SetCursorVisibility(true);

        MainMenuPresenter mainMenuPresenter = new MainMenuPresenter(mainMenu);
        OptionsMenuPresenter optionsMenuPresenter = new OptionsMenuPresenter(optionsMenu);
        GaugesPresenter gaugesPresenter = new GaugesPresenter(gauges);
        EndSreenPresenter endGamePresenter = new EndSreenPresenter(endScreen);

        // Set All Buttons SFX in the HUD 
        root.SetButtonsSFX();
    }

    private void StartHUD()
    {
        ActiveElement = gauges.SetEnability(ActiveElement, true);

        this.SetCursorVisibility(false);
        this.Pause(false);
    }

    #endregion
}


public static class UIExtensions
{
    public static VisualElement OpenWidget(this WidgetData.Widgets widget, bool CursorIsEnabled)
    {
        UnityEngine.Cursor.visible = CursorIsEnabled;
        UnityEngine.Cursor.lockState = CursorIsEnabled ? CursorLockMode.None : CursorLockMode.Locked;

        WidgetData.WidgetDictionary[widget].SetEnability(null, true);
        return WidgetData.WidgetDictionary[widget];
    }
    public static VisualElement OpenWidget(this WidgetData.Widgets widget)
    {
        WidgetData.WidgetDictionary[widget].SetEnability(true);
        return WidgetData.WidgetDictionary[widget];
    }

    public static void SetButtonsSFX(this VisualElement root)
    {
        root.Query<Button>().ForEach(button =>
        {
            button.ButtonClickSFX();
            button.ButtonHoverSFX();
        });
    }
    public static void ButtonHoverSFX(this Button button, SoundData.SoundEnum sfx = SoundData.SoundEnum.ButtonHover)
    {
        button.RegisterCallback<PointerEnterEvent>(evt => AudioManager.OnSFXCall(sfx));
    }

    public static void ButtonClickSFX(this Button button, SoundData.SoundEnum sfx = SoundData.SoundEnum.ButtonClick)
    {
        button.RegisterCallback<ClickEvent>(evt => AudioManager.OnSFXCall(sfx));
    }

    public static void SetPreviewWorldPosition(this VisualElement visualElement, Vector3 position)
    {
        visualElement.transform.position = position;
    }

    public static void SetCursorVisibility<T>(this T t, bool visible)
    {
        UnityEngine.Cursor.visible = visible;
        UnityEngine.Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void Pause<T>(this T t, bool isPause)
    {
        Time.timeScale = isPause ? .0f : 1.0f;
    }

    public static void SetColor(this VisualElement visualElement, Color color)
    {
        visualElement.style.color = color;
    }

    public static bool IsEnable(this VisualElement visualElement)
    {
        return visualElement.style.display == DisplayStyle.Flex;
    }

    public static void SetEnability(this VisualElement visualElement, bool isEnable)
    {
        visualElement.style.display = isEnable ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public static VisualElement SetEnability(this VisualElement visualElement, VisualElement lastElement, bool isEnable)
    {
        lastElement = lastElement ?? HUD.ActiveElement;

        if (isEnable && lastElement != null)
            lastElement.style.display = DisplayStyle.None;

        visualElement.style.display = isEnable ? DisplayStyle.Flex : DisplayStyle.None;

        return HUD.ActiveElement = visualElement;
    }
}