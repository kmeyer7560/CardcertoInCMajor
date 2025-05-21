using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public RectTransform mainMenu;
    public float moveDuration = 0.8f;
    public ParticleSystem ps;
    public GameObject vignette;
    private RectTransform vtransform;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        vignette.SetActive(false);
        mainMenu = GameObject.Find("MainImage").GetComponent<RectTransform>();
        vtransform = vignette.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainCameraPosition();
        }
    }

    public void StartGame()
    {
        vignette.SetActive(true);
        Debug.Log("start game");
        if (ps != null)
        {
            ps.Play();
        }
        StartCoroutine(StartGameRoutine());
        StartCoroutine(SmoothShrink(vtransform, 7f, 40f));
    }

    IEnumerator SmoothShrink(RectTransform vtransform, float duration = 1f, float minScale = 0.1f)
    {
        Vector3 initialScale = vtransform.localScale;
        Vector3 targetScale = Vector3.one * minScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            vtransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        vtransform.localScale = targetScale;
    }

    private IEnumerator StartGameRoutine()
    {
        anim.SetTrigger("shake");
        yield return new WaitForSeconds(0f);
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
        return -(Mathf.Cos(Mathf.PI * t) - 1) / 2f;
    }
}
