using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToModeringScene : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Dragon");
    }
}
