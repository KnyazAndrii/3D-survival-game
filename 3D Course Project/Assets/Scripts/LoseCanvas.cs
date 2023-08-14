using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCanvas : MonoBehaviour
{
    public void RetryLevel()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
}
