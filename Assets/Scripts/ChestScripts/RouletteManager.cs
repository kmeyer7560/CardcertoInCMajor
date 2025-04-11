using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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
    private const float cardSpacing = -.215f;
    private const float cardScale = 0.18f;

    public ChestInteraction chestInteraction;
    public GameObject stopper;
    
    void Start()
    {
        stopper.SetActive(false);
        GameObject chest = GameObject.FindGameObjectWithTag("Chest");
        chestInteraction = chest.GetComponent<ChestInteraction>();

    }
    private void Awake()
    {
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

            Renderer renderer = card.GetComponent<Renderer>();
            renderer.material.renderQueue = 3000;
        }
    }

    public void GetChestInteraction(GameObject chest)
    {
        chestInteraction = chest.GetComponent<ChestInteraction>();
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
        chestInteraction.DestroyChest();
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
                RewardPrefab winningReward = RewardPrefabs.FirstOrDefault(r => r != null && r.Prefab != null && r.Prefab.name == hitObject.name.Replace("(Clone)", "").Trim());
                
                if (winningReward != null)
                {
                    Debug.Log($"You won: {winningReward.Value} of {winningReward.Suit}");
                    chestInteraction.GiveReward(winningReward.Value, winningReward.Suit);
                    return;
                }
            }
        }

        Debug.Log("Player is not touching any reward");
    }

    private IEnumerator MakeCardsDisappear()
    {
        stopper.SetActive(false);
        float fadeDuration = 0.1f;

        Renderer[] cardRenderers = rewardContainer.GetComponentsInChildren<Renderer>();
        SpriteRenderer[] spriteRenderers = rewardContainer.GetComponentsInChildren<SpriteRenderer>();
        Image[] images = rewardContainer.GetComponentsInChildren<Image>();
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = 1 - (elapsedTime / fadeDuration);
            
            foreach (Renderer renderer in cardRenderers)
            {
                if (renderer != null && renderer.material != null)
                {
                    Color color = renderer.material.color;
                    color.a = alpha;
                    renderer.material.color = color;
                }
            }

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = alpha;
                    spriteRenderer.color = color;
                }
            }

            foreach (Image image in images)
            {
                if (image != null)
                {
                    Color color = image.color;
                    color.a = alpha;
                    image.color = color;
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        foreach (Transform child in rewardContainer)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void StartSpin()
    {
        stopper.SetActive(true);
        stopper.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane-3f));
        foreach (Transform child in rewardContainer)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        List<RewardPrefab> rewards = GenerateWeightedRewardList(150);
        InstantiateRewards(rewards);

        float verticalOffset = 0f;
        rewardContainer.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        
        rewardContainer.localPosition += new Vector3(0, 0, 1);

        StartCoroutine(SpinRewards());
    }
}
