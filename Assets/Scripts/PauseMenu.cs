using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    public TextMeshProUGUI BackButton;
    public TextMeshProUGUI Volume;
    private int selected;
    public VertexGradient dark;
    public VertexGradient light;
    private int volumeSlider=2;
    public GameObject volume0;
    public GameObject volume1;
    public GameObject volume2;
    public GameObject volume3;
    public GameObject volume4;
    public AudioMixer mixer;
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        //pauseMenu.SetActive(false);
    }

    void Update()
    {
        switch(volumeSlider)
        {
            case 0:
                volume0.SetActive(true);
                volume1.SetActive(false);
                volume2.SetActive(false);
                volume3.SetActive(false);
                volume4.SetActive(false);
                break;
            case 1:
                volume0.SetActive(false);
                volume1.SetActive(true);
                volume2.SetActive(false);
                volume3.SetActive(false);
                volume4.SetActive(false);
                break;
            case 2:
                volume0.SetActive(false);
                volume1.SetActive(false);
                volume2.SetActive(true);
                volume3.SetActive(false);
                volume4.SetActive(false);
                break;
            case 3:
                volume0.SetActive(false);
                volume1.SetActive(false);
                volume2.SetActive(false);
                volume3.SetActive(true);
                volume4.SetActive(false);
                break;
            case 4:
                volume0.SetActive(false);
                volume1.SetActive(false);
                volume2.SetActive(false);
                volume3.SetActive(false);
                volume4.SetActive(true);
                break;
        }
        if(pauseMenu.activeSelf && Input.GetKeyDown("s") && selected <2)
        {
            selected++;
        }
        if(pauseMenu.activeSelf && Input.GetKeyDown("w") && selected >-1)
        {
            selected--;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
        switch(selected)
        {
            case 0:
                BackButton.colorGradient = light;
                Volume.colorGradient = dark;
                break;
            case 1:
                Volume.colorGradient = light;
                BackButton.colorGradient = dark;
                break;
        }
        if(Input.GetKeyDown(KeyCode.Space) && pauseMenu.activeSelf && selected == 0)
        {
                pauseMenu.SetActive(false);
        }
        if(pauseMenu.activeSelf && selected == 1)
        {
            if(Input.GetKeyDown("d")&& volumeSlider < 6)
            {
                volumeSlider++;
            }
            if(Input.GetKeyDown("d")&& volumeSlider > -1)
            {
                volumeSlider--;
            }
        }
    }
}
