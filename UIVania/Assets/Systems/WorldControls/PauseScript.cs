using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool GamePaused = false;
    private static int itemsPausing = 0;

    public void PauseGame()
    {
        GamePaused = true;
        itemsPausing++;
    }

    public void UnpauseGame()
    {
        itemsPausing--;
        if (itemsPausing == 0)
        {
            StartCoroutine(Unpause());
        }
    }

    IEnumerator Unpause()
    {
        yield return new WaitForSeconds(.01f);
        GamePaused = false;
    }
}
