using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RouletteManager : MonoBehaviour
{
    [System.Serializable]
    public class RewardPrefab
    {
        public string Value;
        public string Suit;
        public int Weight;
        public GameObject Prefab;
    }

    public RewardPrefab[] RewardPrefabs;

    public Transform rewardContainer;
    public Transform playerTransform;

    private const float cardWidth = 0.4f;
    private const float cardSpacing = 0f;
    private const float cardScale = 0.4f;

    private void Awake()
    {
        // Set weights based on card values
        foreach (var reward in RewardPrefabs)
        {
            switch (reward.Value)
            {
                case "2":
                    reward.Weight = 40;
                    break;
                case "4":
                    reward.Weight = 30;
                    break;
                case "8":
                    reward.Weight = 20;
                    break;
                case "Ace":
                    reward.Weight = 10;
                    break;
            }
        }
    }

    private List<RewardPrefab> GenerateWeightedRewardList(int count)
    {
        List<RewardPrefab> weightedList = new List<RewardPrefab>();
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int totalWeight = RewardPrefabs.Sum(r => r.Weight);
            int randomWeight = random.Next(totalWeight);
            int cumulativeWeight = 0;

            foreach (RewardPrefab rewardPrefab in RewardPrefabs)
            {
                cumulativeWeight += rewardPrefab.Weight;
                if (randomWeight < cumulativeWeight)
                {
                    weightedList.Add(rewardPrefab);
                    break;
                }
            }
        }

        return weightedList;
    }

    private void InstantiateRewards(List<RewardPrefab> rewards)
    {
        float totalWidth = (rewards.Count * cardWidth) + ((rewards.Count - 1) * cardSpacing);
        float startX = -totalWidth / 2;

        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject card = Instantiate(rewards[i].Prefab, rewardContainer);
            card.transform.localScale = new Vector3(cardScale, cardScale, 1f);
            float xPosition = startX + (i * (cardWidth + cardSpacing));
            card.transform.localPosition = new Vector3(xPosition, 0, 0);

            BoxCollider2D collider = card.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    private IEnumerator SpinRewards()
    {
        float duration = 5f;
        int cardCount = rewardContainer.childCount;

        float totalWidth = ((cardCount * cardWidth) + ((cardCount - 1) * cardSpacing)) / 0.25f;
        float startX = totalWidth / 1;
        float endX = -totalWidth / 1;

        Vector3 startPosition = new Vector3(startX, rewardContainer.localPosition.y, rewardContainer.localPosition.z);
        Vector3 endPosition = new Vector3(endX, rewardContainer.localPosition.y, rewardContainer.localPosition.z);

        rewardContainer.localPosition = startPosition;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutCubic(t);
            rewardContainer.localPosition = Vector3.Lerp(startPosition, endPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rewardContainer.localPosition = endPosition;
        yield return new WaitForSecondsRealtime(0.5f);
        DetermineWinningReward();
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;

        StartCoroutine(MakeCardsDisappear());
    }




    private float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    private void DetermineWinningReward()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerTransform.position, 0.5f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.transform.parent == rewardContainer)
            {
                GameObject hitObject = hitCollider.gameObject;
                RewardPrefab winningReward = RewardPrefabs.FirstOrDefault(r => r.Prefab.name == hitObject.name.Replace("(Clone)", "").Trim());
                
                if (winningReward != null)
                {
                    Debug.Log($"You won: {winningReward.Value} of {winningReward.Suit}");
                    //GetComponent<ChestInteraction>.
                    return;
                }
            }
        }

        Debug.Log("Player is not touching any reward");
    }

    private IEnumerator MakeCardsDisappear()
    {
        float fadeDuration = .1f;

        Renderer[] cardRenderers = rewardContainer.GetComponentsInChildren<Renderer>();
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = 1 - (elapsedTime / fadeDuration);
            
            foreach (Renderer renderer in cardRenderers)
            {
                Color color = renderer.material.color;
                color.a = alpha;
                renderer.material.color = color;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartSpin()
    {
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }

        List<RewardPrefab> rewards = GenerateWeightedRewardList(50);
        InstantiateRewards(rewards);

        float verticalOffset = 0f;
        rewardContainer.position = playerTransform.position + new Vector3(0, verticalOffset, 0);
        
        rewardContainer.localPosition += new Vector3(0, 0, 5);

        StartCoroutine(SpinRewards());
    }
}