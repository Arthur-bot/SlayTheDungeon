using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum KeyWord
{
    Attack,
    Defend,
    Poison,
    Draw,
    Generation,
    Combo,
    Energy,
    Heal,
    Fury,
    PayLife
}

public class Boss : Enemy
{
    private bool isPlaying;
    [SerializeField] private CardUI cardPlayedPrefab;
    [SerializeField] private Transform cardPlayedRoot;
    private int energy = 3;
    private BaseAI ai;
    private List<CardData> deck = new List<CardData>();
    private List<CardData> discard = new List<CardData>();
    private List<CardData> hand = new List<CardData>();

    public List<CardData> Hand { get => hand; set => hand = value; }
    public int Energy { get => energy; set => energy = value; }
    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

    public override void SetupEnemy()
    {
        base.SetupEnemy();
        foreach (CardData card in EnnemyData.EnnemyDeck)
        {
            deck.Add(Instantiate(card));
        }
        ai = Instantiate(EnnemyData.Ai);
        ai.Owner = this;
        Debug.Log(ai.Owner);
        ai.Init();
        DrawCards(5);
    }
    public override void PlayTurn()
    {
        //reset energy
        energy = 3;

        //setup target to use cards properly
        TargetingSystem.Instance.SetTarget(this);
        isPlaying = true;
        ai.TakeDecision();
    }

    public void DiscardHand()
    {
        foreach (CardData card in hand)
        {
            discard.Add(card);
        }
        hand.Clear();
    }

    public bool PlayACard(CardData card)
    {
        if (energy >= card.Cost)
        {
            CardUI playedCard = Instantiate(cardPlayedPrefab, Vector2.zero, Quaternion.identity, cardPlayedRoot);
            playedCard.transform.localScale = Vector3.one / 2.0f;
            playedCard.SetupCard(card);
            playedCard.transform.DOLocalMoveY(playedCard.transform.localPosition.y + 30.0f, 0.5f).OnComplete
                (() => { StartCoroutine(FadeCard(playedCard.GetComponent<CanvasGroup>(), 0.5f)); });
            energy -= card.Cost;
            card.Use(false);
            hand.Remove(card);
            if (!card.LimitedUse || card.NbUse > 0)
                discard.Add(card);
            return true;
        }
        return false;
    }
    private void DrawOneCard()
    {
        if (deck.Count == 0 && discard.Count == 0) return;
        if (deck.Count == 0)
        {
            Shuffle();
        }
        int randomIndex = Random.Range(0, deck.Count);
        CardData selected = deck[randomIndex];
        deck.RemoveAt(randomIndex);
        hand.Add(selected);
    }

    private void Shuffle()
    {
        foreach (CardData card in discard)
        {
            deck.Add(card);
        }
        discard.Clear();
    }
    protected override void OnDeath()
    {
        SceneManager.LoadScene(0);
    }

    public override void DrawCards(int value)
    {
        for (int i = 0; i < value; i++)
        {
            DrawOneCard();
        }
    }
    public override void GetEnergy(int value)
    {
        energy += value;
    }
    public override void AddCards(List<CardData> toAdd)
    {
        foreach(CardData card in toAdd)
        {
            hand.Add(Instantiate(card));
        }
    }

    IEnumerator FadeCard(CanvasGroup card, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            card.alpha = 1 - i;
            yield return null;
        }
        Destroy(card.gameObject);
    }
}
