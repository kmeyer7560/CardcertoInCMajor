using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour
{
    public float detectionRange = 5f;
    private bool playerInRange = false;
    public LayerMask playerLayer;

    public ScrollRect scrollRect;
    public float spinDuration = 5f;
    public float spinSpeed = 2000f;
    public AnimationCurve spinCurve;

    private bool isSpinning = false;

    private void Update()
    {
        Vector2 boxSize = new Vector2(detectionRange * 2, detectionRange * 2);
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, playerLayer);
        playerInRange = (hit != null);

        if(playerInRange && Input.GetKeyDown("space"))
        {
            Gamble();
        }
    }

    public void Gamble()
    {
        if(!isSpinning)
        {
            StartCoroutine(SpinRoulette());
        }    
    }
    private IEnumerator SpinRoulette()
    {
        isSpinning = true;
        float elapsedTime = 0f;
        float startPosition = scrollRect.horizontalNormalizedPosition;
        float targetPosition = Random.value;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            
            float currentPosition = Mathf.Lerp(startPosition, targetPosition, curveValue);
            scrollRect.horizontalNormalizedPosition = currentPosition;

            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
        isSpinning = false;

        // Handle item selection logic here
    }
}
