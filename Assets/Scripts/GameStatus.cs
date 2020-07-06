using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStatus : MonoBehaviour
{
    // System.Flags changes how eGameStates are viewed in the Inspector and lets multiple 
    //  values be selected simultaneously (similar to how Physics Layers are selected).
    // It's only valid for the game to ever be in one state, but I've added System.Flags
    //  here to demonstrate it and to make the ActiveOnlyDuringSomeGameStates script easier
    //  to view and modify in the Inspector.
    // When you use System.Flags, you still need to set each enum value so that it aligns 
    //  with a power of 2. You can also define enums that combine two or more values,
    //  for example the all value below that combines all other possible values.
    [System.Flags]
    public enum eGameState
    {
        // Decimal      // Binary
        none = 0,       // 00000000
        loading = 1,
        mainMenu = 2,   // 00000001
        inGame = 4,
        transition = 8,  // 00001000
        gameOver = 16,  // 00010000
        closing = 32,
        all = 0xFFFFFFF // 11111111111111111111111111111111
    }

    static private GameStatus _S;

    // ---------------- Static Section ---------------- //

    /// <summary>
    /// <para>This static private property provides some protection for the Singleton _S.</para>
    /// <para>get {} does return null, but throws an error first.</para>
    /// <para>set {} allows overwrite of _S by a 2nd instance, but throws an error first.</para>
    /// <para>Another advantage of using a property here is that it allows you to place
    /// a breakpoint in the set clause and then look at the call stack if you fear that 
    /// something random is setting your _S value.</para>
    /// </summary>
    static public GameStatus S
    {
        get
        {
            if (_S == null)
            {
                Debug.LogError("GameStatus:S getter - Attempt to get value of S before it has been set.");
                return null;
            }
            return _S;
        }
        set
        {
            if (_S != null)
            {
                Debug.LogError("GameStatus:S setter - Attempt to set S when it has already been set.");
            }
            _S = value;
        }
    }

    static private bool _PAUSED = false;
    static private eGameState _GAME_STATE = eGameState.loading;

    static public bool PAUSED
    {
        get
        {
            return _PAUSED;
        }
        private set
        {
            if (value != _PAUSED)
            {
                _PAUSED = value;
                // Need to update all of the handlers
                // Any time you use a delegate, you run the risk of it not having any handlers
                //  assigned to it. In that case, it is null and will throw a null reference
                //  exception if you try to call it. So *any* time you call a delegate, you 
                //  should check beforehand to make sure it's not null.
                if (PAUSED_CHANGE_DELEGATE != null)
                {
                    PAUSED_CHANGE_DELEGATE();
                }
            }

        }
    }

    static public eGameState GAME_STATE
    {
        get
        {
            return _GAME_STATE;
        }
        set
        {
            if (value != _GAME_STATE)
            {
                _GAME_STATE = value;
                // Need to update all of the handlers
                // Any time you use a delegate, you run the risk of it not having any handlers
                //  assigned to it. In that case, it is null and will throw a null reference
                //  exception if you try to call it. So *any* time you call a delegate, you 
                //  should check beforehand to make sure it's not null.
                if (GAME_STATE_CHANGE_DELEGATE != null)
                {
                    GAME_STATE_CHANGE_DELEGATE();
                }
            }
        }
    }





    public delegate void CallbackDelegate(); // Set up a generic delegate type.
    static public event CallbackDelegate GAME_STATE_CHANGE_DELEGATE;
    static public event CallbackDelegate PAUSED_CHANGE_DELEGATE;




    [Header("These reflect static fields and are otherwise unused")]
    [SerializeField]
    [Tooltip("This private field shows the game state in the Inspector and is set by the "
        + "GAME_STATE_CHANGE_DELEGATE whenever GAME_STATE changes.")]
    protected eGameState _gameState;





    [SerializeField]
    [Tooltip("This private field shows the game state in the Inspector and is set by the "
    + "PAUSED_CHANGE_DELEGATE whenever PAUSED changes.")]
    protected bool _paused;



    [Header("Set in Inspector")]
    [Tooltip("This sets the UIScriptableObj to be used throughout the game.")]
    public UIScriptableObj UserInterfaceSO;


    public PlayerProgress playerProgress;

    public bool hardLevel = true;



    private void Awake()
    {


        S = this;


        // Rather than use the anonymous delegate that was here previously, I've instead
        //  created a method that conforms to the GAME_STATE_CHANGE_DELEGATE, thereby 
        //  avoiding the memory issues that can be caused by closures.
        GAME_STATE_CHANGE_DELEGATE += GameStateChanged;
        PAUSED_CHANGE_DELEGATE += PauseChanged;

        // This strange use of _gameState and _paused as an intermediary in the following 
        //  lines is solely to stop the Warning from popping up in the Console telling you 
        //  that _gameState was assigned but not used.
        _gameState = eGameState.loading;
        GAME_STATE = _gameState;
        _paused = false;
        PauseGame(_paused);

        NotificationCenter.DefaultCenter().AddObserver(this, "PlayLevel");
        NotificationCenter.DefaultCenter().AddObserver(this, "LevelComplete");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameOver");
        NotificationCenter.DefaultCenter().AddObserver(this, "PauseGameToggle");



    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public void PauseGameToggle()
    {
        PauseGame(!PAUSED);
    }

    public void PauseGame(bool toPaused)
    {
        PAUSED = toPaused;
        if (PAUSED)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void GameStateChanged()
    {
        this._gameState = GameStatus.GAME_STATE;
    }

    void PauseChanged()
    {
        this._paused = GameStatus.PAUSED;
    }

    private void OnDestroy()
    {
        GAME_STATE_CHANGE_DELEGATE -= GameStateChanged;
        PAUSED_CHANGE_DELEGATE -= PauseChanged;
        GameStatus.GAME_STATE = GameStatus.eGameState.closing;
    }



    static public UIScriptableObj UISO
    {
        get
        {
            if (S != null)
            {
                return S.UserInterfaceSO;
            }
            return null;
        }
    }


    eGameState nextStep = eGameState.none;

    public void GoToMainScreen()
    {
        nextStep = eGameState.mainMenu;
        StartCoroutine(UnloadScenes());
    }

    public void GoToGame()
    {
        nextStep = eGameState.inGame;
        StartCoroutine(LoadAsyncScene());
    }

    public void SavePlayerProgress()
    {
        SaveSystem.SavePlayer(playerProgress);
    }



    public void LevelCompleted(double score)
    {
        GAME_STATE = eGameState.gameOver;


    }

    public void RestartLevel()
    {

        GAME_STATE = eGameState.inGame;
    }

    IEnumerator StartGame()
    {
        /*while (!Transitions.S.FadeIn())
        {
            yield return null;
        }*/


        yield return new WaitForSeconds(0.1f);
        ReadPlayerProgress();




        while (!Transitions.S.FadeOut())
        {
            yield return null;
        }
        GAME_STATE = eGameState.mainMenu;

    }


    IEnumerator CRTransition()
    {
        while (!Transitions.S.FadeIn())
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        GAME_STATE = nextStep;
        yield return new WaitForSeconds(0.4f);

        while (!Transitions.S.FadeOut())
        {
            yield return null;
        }

    }




    IEnumerator LoadAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        while (!Transitions.S.FadeIn())
        {
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("DuckHunt", LoadSceneMode.Additive);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Transitions.S.progress = asyncLoad.progress;
            yield return null;
        }
        GAME_STATE = eGameState.inGame;

        while (!Transitions.S.FadeOut())
        {
            yield return null;
        }
        NotificationCenter.DefaultCenter().PostNotification(this, "StartCountDown");
    }

    public void GoBackFromLevel()
    {

        StartCoroutine(UnloadScenes());
    }

    IEnumerator UnloadScenes()
    {

        while (!Transitions.S.FadeIn())
        {
            yield return null;
        }


        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("DuckHunt");
        while (!asyncUnload.isDone)
        {
            yield return null;
        }


        PauseGame(false);
        GAME_STATE = eGameState.mainMenu;
        while (!Transitions.S.FadeOut())
        {
            yield return null;
        }






    }




    /// <summary>
    ///Read player progress
    /// </summary>


    void ReadPlayerProgress()
    {
        playerProgress = SaveSystem.LoadPlayer();

    }

    public void DeleteProgress()
    {

        playerProgress = SaveSystem.DeleteProgress();

    }




    public void BackToMenu()
    {
        GAME_STATE = eGameState.mainMenu;
    }

    void LevelComplete(Notification newLevelTime)
    {
        float newtime = (newLevelTime.data as LevelTime).newTime;
        newtime = ((hardLevel) ?UISO.HardTime:UISO.MedTime)-newtime;
        bool record = playerProgress.UpdateLevel(hardLevel, newtime);
        Debug.Log("Level Completed");
        string gameOverMsg;
        if (record)
        {
            gameOverMsg = "NEW RECORD!\n YOU DID IT IN\n" + newtime.ToString("##.##") + " seconds";
            SavePlayerProgress();
        }
        else
        {
            float oldtime = (hardLevel) ? playerProgress.secondsHard : playerProgress.secondsMed;
            gameOverMsg = "GOOD PLAY!\n BUT THE RECORD IS IN\n" + oldtime.ToString("##.##") + " seconds\n Try again!";
        }
        GAME_STATE = eGameState.gameOver;
        NotificationCenter.DefaultCenter().PostNotification(this, "ResultMsg", gameOverMsg);
    }

    void GameOver()
    {
        GAME_STATE = eGameState.gameOver;
        string gameOverMsg = "OH! GAMEOVER!\n You didn't in time";
        NotificationCenter.DefaultCenter().PostNotification(this, "ResultMsg", gameOverMsg);
    }
}
