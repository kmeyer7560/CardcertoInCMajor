using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayButton : MonoBehaviour
{
    public Transform mainMenu;
    public void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("ForestMain");
        mainMenu = GameObject.Find("MainImage").transform;
    }

    public void SettingsCameraPosition()
    {
        mainMenu.transform.position = Vector3.MoveTowards(mainMenu.transform.position, new Vector3(1f, 1f, 1f), 2*Time.deltaTime);
    }

    public void MainCameraPosition()
    {
        mainMenu.transform.position = Vector3.MoveTowards(mainMenu.transform.position, new Vector3(3f, 3f, 3f), 2* Time.deltaTime);
    }
}
