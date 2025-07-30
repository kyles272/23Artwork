using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    /* Store these information about the game:
        * - Current level or scene
        * - Game state
    */

    public string currentLevel;
    public bool isGamePaused = false;
}
