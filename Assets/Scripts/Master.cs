using Assets.Scripts.Bayeux;
using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models.Palettes;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DialogueManager;

public class Master : MonoBehaviour {

    public static Master GM;
    public GameObject Player;
    public GameObject SplashScreen;
    public GameObject CurrentParent;
    public Texture2D CanvasTexture;
    public ScenarioData ScenarioData = null;
    public Location Location;
    public LocationInfo CurrentLocation = null;
    public DialogueManager DialogueManager;

    public DrawerLogic Contents;

    public GameObject Tooltip;
    public GameObject PomaUI;
    public Button PomaButton;
    public bool PomaButtonOver;

    //public ListButtonScript ListButton;
    //public Player PlayerScript;
    public GameObject PlayerInstance;
    public Texture2D MasterTexture;
    public GameObject LoadingScreen;
    public GameObject LocationInterface;
    public GameObject PlayerBadge;
    public ClockScript Clock;
    public AudioSource AudioSrc;

    public int LevelCounter;

    public SfxScript Sfx;
    public bool LoadingScreenOn;

    public bool RenderLowResAssets;

    private bool AllMenuOverlaysOff() => !DialogueManager.DialogueEnabled && !DialogueManager.MenuEnabled && !LoadingScreenOn;
    public bool TooltipEnabled() => AllMenuOverlaysOff() && !Contents.Open ;
    public bool DrawerTooltipEnabled() => AllMenuOverlaysOff() && Contents.Open;

    private GameObject _parentRef;
    private BackgroundMusic _music;
    private int LevelChangeFlag = int.MaxValue;
    private bool _justStarted = true;

    void Awake () {
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(gameObject);

            // If the screen is running below 1920*1080, scale procedural assets
            if (Screen.currentResolution.width < 1920)
            {
                RenderLowResAssets = true;
                Screen.SetResolution(1280, 720, true);
            }

            LocalRandom.ConfigureRandom(new System.Random());

            _music = GetComponent<BackgroundMusic>();
            AudioSrc = GetComponent<AudioSource>();

            LevelCounter = 0;

            PomaButton.onClick.AddListener(() => {
                TriggerPoma();
            });

            ConfigureColors();

            LevelSetup();            
        }            
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void ActivateSplashScreen()
    {
        SplashScreen.SetActive(true);
    }

    public void RemoveSplashScreen()
    {
        SplashScreen.SetActive(false);
        _justStarted = false;
        AudioSrc.clip = _music.Clips[0];
        AudioSrc.Play();
        Clock.StartTimer(Levels.LevelList[LevelCounter].SecondsAvailable);
    }

    public static void ConfigureColors()
    {
        var textureTools = new TextureTools(OverlayRef.Am6RefDictWHash);
        ColorsList.OverlayProps = textureTools.StringToColor(NamesList.OverlayProps);
        ColorsList.OverlayWallProps = textureTools.StringToColor(NamesList.OverlayWallProps);
        ColorsList.OverlayCharacter = textureTools.StringToColor(NamesList.OverlayCharacter);
        ColorsList.OverlayMapLocation = textureTools.StringToColor(NamesList.OverlayMapLocation);
        ColorsList.OverlayMapSpawn = textureTools.StringToColor(NamesList.OverlayMapSpawn);

        Palettes.BgColors = Palettes.BgStringColors.Select(x => textureTools.StringToColor32(x)).ToList();
        Palettes.Replacement0 = textureTools.ConvertPalette(Palettes.Replacement0String.AddHashes());
        Palettes.Replacement1 = textureTools.ConvertPalette(Palettes.Replacement1String.AddHashes());
        Palettes.Replacement2 = textureTools.ConvertPalette(Palettes.Replacement2String.AddHashes());
    }

    public Transform GetSceneTransform()
    {
        return _parentRef.transform;
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }

    public void Start()
    {
        Location.CreateLocation(CurrentLocation, GetSceneTransform());
    }

    public void RestartButton()
    {
        LevelCounter = 0;
        CallNextLevel();
    }

    public void NextLevel()
    {
        LocationInterface.SetActive(false);
        LevelCounter++;
        if (LevelCounter == Levels.LevelList.Length)
        {
            DialogueManager.GameEnd();
        }
        else
        {
            CallNextLevel();
        }
    }

    //public void ToggleInterface(bool on)
    //{
    //    LocationInterface.SetActive(on);
    //    PomaButton.enabled = on;
    //}

    public void ChangeScene(LocationInfo To, bool restart = false)
    {
        // Save the current playback time & stop music
        CurrentLocation.MusicPlaybackTime = AudioSrc.time;
        AudioSrc.Stop();

        // Check where the player is coming from and save it
        var playerLoc = restart ? ScenarioData.Main.SpawnPoint
            : CurrentLocation.CoordsOnMainMap;

        // LOCATION CHANGE!
        CurrentLocation = To;
        SceneInit();

        // Restart music        
        AudioSrc.clip = _music.Clips[CurrentLocation.MusicClipIndex];
        AudioSrc.time = CurrentLocation.MusicPlaybackTime;
        AudioSrc.Play();

        ToggleLocation(To.IsMap);

        if (To.IsMap)
        {
            HidePoma();
            DialogueManager.HideInfo();
            SceneManager.LoadScene(NamesList.MainScenario, LoadSceneMode.Single);
            Location.CreateLocation(CurrentLocation, GetSceneTransform());
            PlayerInstance.transform.position = playerLoc;
        }
        else
        {
            SceneManager.LoadScene(NamesList.Location, LoadSceneMode.Single);
            Location.CreateLocation(CurrentLocation, GetSceneTransform());
            DialogueManager.ShowInfo(
                TextResources.LocationInfo(CurrentLocation.Person.Name));
        }
    }

    public void ArrestAttempt()
    {
        var success = CurrentLocation.Person.IsThief;
        var function = success ? DiagButtonsFunction.ArrestSuccess :
            DiagButtonsFunction.ArrestFailed;

        var arrestMsg = TextResources.GetArrestMessage(
                success,
                GM.CurrentLocation.Person.Name
            );

        AudioSrc.Stop();

        if (success)
            Sfx.Success();
        else
            Sfx.Fail();

        DialogueManager.CaptureMenu(function, arrestMsg);
    }

    public void TriggerPoma()
    {
        GM.PomaUI.gameObject.SetActive(!GM.PomaUI.gameObject.activeSelf);
    }

    public void HidePoma()
    {
        GM.PomaUI.gameObject.SetActive(false);
    }

    public void WDCTimeout()
    {
        AudioSrc.Stop();
        Sfx.TimeUp();
        DialogueManager.CaptureMenu(DiagButtonsFunction.ArrestFailed,
            TextResources.ArrestTimeout);
    }

    //public void SetupList()
    //{
    //    _listInstance.name = "List";
    //    _listInstance.transform.SetParent(gameObject.transform);
    //    ListButton = List.GetComponent<ListButtonScript>();
    //}

    public void ToggleLocation(bool isMap = true)
    {
        PlayerInstance.SetActive(isMap);
        PlayerBadge.SetActive(!isMap);
        LocationInterface.SetActive(!isMap);
    }

    private void LevelSetup()
    {
        if (_justStarted)
            ActivateSplashScreen();

        //Debug.Log("init GM");
        var scenarioGenerator = new ScenarioSetup(_music.Clips.Length);
        var level = Levels.LevelList[LevelCounter];
        ScenarioData = scenarioGenerator.GenerateScenarioData(level);
        // TODO depending on the setup, get the location where the player is supposed to start off
        CurrentLocation = ScenarioData.Main;

        PlayerInstance = Instantiate(Player, ScenarioData.Main.SpawnPoint, Quaternion.identity);
        PlayerInstance.transform.SetParent(gameObject.transform);
        PlayerInstance.name = NamesList.Player;

        //PlayerScript = PlayerInstance.GetComponent<Player>();

        SceneInit();

        ToggleLocation(true);
        DialogueManager.HideInfo();

        if (!_justStarted)
        {
            AudioSrc.clip = _music.Clips[0];
            AudioSrc.Play();
            Clock.StartTimer(level.SecondsAvailable);
        }

        SetLoadingScreenOff();
        PomaUI.GetComponentInChildren<Text>().text = GM.ScenarioData.PomaText;
    }

    private void SceneInit()
    {
        if (_parentRef != null)
        {
            Destroy(_parentRef);
        }
        _parentRef = Instantiate(CurrentParent, new Vector2(0, 0), Quaternion.identity);
        _parentRef.transform.SetParent(transform);
    }

    private void TearDown()
    {
        Destroy(PlayerInstance);
        ScenarioData = null;
        AudioSrc.Stop();
    }

    private void SetLoadingScreen()
    {
        LoadingScreenOn = true;
        Sfx.Stop();
        DialogueManager.MenuScreen.SetActive(false);
        LoadingScreen.SetActive(true);
    }

    private void SetLoadingScreenOff()
    {
        LoadingScreenOn = false;
        LoadingScreen.SetActive(false);
    }

    private void CallNextLevel()
    {
        SetLoadingScreen();
        LevelChangeFlag = Time.frameCount;
    }

    private void LoadLevel()
    {
        TearDown();
        LevelSetup();
        ChangeScene(ScenarioData.Main, true);
    }

    private void Update()
    {
        // TODO can't get the loading screen to display properly is restart starts too early.
        // find a better fix
        if (Time.frameCount - LevelChangeFlag > 3)
        {
            LevelChangeFlag = int.MaxValue;
            LoadLevel();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (AllMenuOverlaysOff())
                TriggerPoma();
        }

        if (SplashScreen.activeSelf && Input.GetKeyUp(KeyCode.Space))
        {
            RemoveSplashScreen();
        }

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}