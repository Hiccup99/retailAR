using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void Load(int x)
    {
        if (x == 5)
            Application.Quit();
        else
        {
            SceneManager.LoadScene(x);
        }
    }
}
