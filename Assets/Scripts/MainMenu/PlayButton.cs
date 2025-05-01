using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public RectTransform mainMenu;
    public float moveDuration = .8f; 

    void Start()
    {
        mainMenu = GameObject.Find("MainImage").GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MainCameraPosition();
        }
    }

    public void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("ForestMain");
    }

    public void SettingsCameraPosition()
    {
        Debug.Log("move settings");
        StartCoroutine(MoveTransition(new Vector2(0f, -323f)));
    }

    public void MainCameraPosition()
    {
        Debug.Log("move main");
        StartCoroutine(MoveTransition(new Vector2(0f, 323f)));
    }

    IEnumerator MoveTransition(Vector2 targetPosition)
    {
        Vector2 startPosition = mainMenu.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            t = EaseInOutSine(t);

            mainMenu.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        mainMenu.anchoredPosition = targetPosition;
    }

    float EaseInOutSine(float t)
    {
        return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
    }
}
