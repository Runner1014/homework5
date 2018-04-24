using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    bool newStart = true;
    int round = 1;
    Rect buttonContainer = new Rect(Screen.width / 2 - 40, Screen.height / 2 - 100, 80, 80);
    Rect labelContainer = new Rect(Screen.width / 2 - 40, Screen.height / 2 - 100, 80, 80);

    // Use this for initialization  
    void Start()
    {

        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    private void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle("button");
        GUIStyle labelStyle = new GUIStyle("Label");
        GUIStyle labelStyle2 = new GUIStyle("Label");
        buttonStyle.fontSize = 25;
        buttonStyle.fontStyle = FontStyle.Bold;
        labelStyle.fontSize = 40;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle2.alignment = TextAnchor.UpperCenter;

        if (action.getMode() == ActionMode.NOTSET)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 95, Screen.height / 2 - 150, 90, 50), "KINEMATIC"))
            {
                action.setMode(ActionMode.KINEMATIC);
            }
            if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 150, 90, 50), "PHYSICS"))
            {
                action.setMode(ActionMode.PHYSICS);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 position = Input.mousePosition;
                action.hit(position);
            }

            GUI.Label(labelContainer, action.GetScore().ToString(), labelStyle);
            GUI.Label(labelContainer, "Round: " + round.ToString(), labelStyle2);
            if (newStart && GUI.Button(buttonContainer, "Start", buttonStyle))
            {
                newStart = false;
                action.setGameState(GameState.ROUND_START);
            }

            if (!newStart && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(buttonContainer, "Next Round"))
            {
                round = round % 3 + 1;
                action.setGameState(GameState.ROUND_START);
            }
        }
    }
}