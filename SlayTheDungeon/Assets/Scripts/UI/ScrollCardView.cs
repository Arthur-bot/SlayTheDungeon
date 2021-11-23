using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCardView : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject templateCardView;
    private List<GameObject> cardViews = new List<GameObject>();
    public void ShowCards(List<CardData> cards)
    {
        while (cardViews.Count < cards.Count)
        {
            cardViews.Add(Instantiate(templateCardView, transform));
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cardViews[i].SetActive(true);
            cardViews[i].GetComponent<CardUI>().SetupCard(cards[i]);
        }
        Invoke("ScrollUp", 0.01f);
    }

    public void ScrollUp()
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }
    public void HideCards()
    {
        foreach(var view in cardViews)
        {
            view.SetActive(false);
        }
    }
}
