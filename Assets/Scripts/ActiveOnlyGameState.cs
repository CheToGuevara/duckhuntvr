using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnlyGameState : MonoBehaviour {

    // eGameStates is a System.Flags enum, so many values can be stored in a single field.


    public enum ePauseEffect
    {
        ignorePause,
        activeWhenPaused,
        activeWhenNotPaused
    }

    // eGameStates is a System.Flags enum, so many values can be stored in a single field.
    [EnumFlags] // This uses the EnumFlagsAttribute from EnumFlagsAttributePropertyDrawer
    public GameStatus.eGameState activeStates = GameStatus.eGameState.all;
    public ePauseEffect pauseEffect = ePauseEffect.ignorePause;
    [Tooltip("Check editorOnly to make this GameObject active only in the Unity editor.")]
    public bool editorOnly = false;

    // Use this for initialization
    public virtual void Start()
    {

        // Also make sure to set self based on the current state when awakened
        isActive();

        // Register this callback with the static public events on GameStatus.
        GameStatus.GAME_STATE_CHANGE_DELEGATE += isActive;
        GameStatus.PAUSED_CHANGE_DELEGATE += isActive;
    }

    protected void OnDestroy()
    {
        // Unregister this callback from the static public events on GameStatus.
        GameStatus.GAME_STATE_CHANGE_DELEGATE -= isActive;
        GameStatus.PAUSED_CHANGE_DELEGATE -= isActive;

    }


    protected virtual void isActive()
    {

        bool shouldBeActive = (activeStates & GameStatus.GAME_STATE) == GameStatus.GAME_STATE;
        if (shouldBeActive)
        {
            // This only comes into play if shouldBeActive is true at this point
            switch (pauseEffect)
            {
                case ePauseEffect.activeWhenNotPaused:
                    shouldBeActive = !GameStatus.PAUSED;
                    break;
                case ePauseEffect.activeWhenPaused:
                    shouldBeActive = GameStatus.PAUSED;
                    break;
            }

            if (editorOnly && !Application.isEditor)
            {
                shouldBeActive = false;
            }
        }
        gameObject.SetActive(shouldBeActive);
    }
}
