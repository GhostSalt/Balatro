using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public CardArea JokerArea;
    public CardArea PlayingCardArea;

    public CardRendererSpawner CardSpawner;
    public PlayingCardRendererSpawner PlayingCardSpawner;

    public void Activate()
    {
        JokerArea.Activate();
        PlayingCardArea.Activate();
    }

    public void Deactivate()
    {
        JokerArea.Deactivate();
        PlayingCardArea.Deactivate();
    }

    public void SetJokers(List<Joker> jokers)
    {
        var rends = new List<CardRenderer>();
        foreach (var joker in jokers)
            rends.Add(CardSpawner.SpawnCard(joker, JokerArea));
        JokerArea.SetCards(rends.ToArray());
    }

    public void SetPlayingCards(List<PlayingCard> cards)
    {
        var rends = new List<PlayingCardRenderer>();
        foreach (var card in cards)
            rends.Add(PlayingCardSpawner.SpawnCard(card, PlayingCardArea));
        PlayingCardArea.SetCards(rends.ToArray());
    }
}
