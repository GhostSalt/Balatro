using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalatroScript : MonoBehaviour
{
    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public CardRendererSpawner CardSpawner;
    public PlayingCardRendererSpawner PlayingCardSpawner;
    public ChipRendererSpawner ChipSpawner;
    public CardArea JokerArea;
    public CardArea PlayingCardArea;
    public Transform BlindLocation;
    public IntroScreen IntroScreen;

    private ChipRenderer CurrentBlind;

    void Awake()
    {
        _moduleID = _moduleIdCounter++;

        JokerArea.SetCards(new CardRenderer[]{

            CardSpawner.SpawnCard("perkeo", JokerArea),
            CardSpawner.SpawnCard("steel joker", JokerArea),
            CardSpawner.SpawnCard("cavendish", JokerArea),
            CardSpawner.SpawnCard("hit the road", JokerArea),
            CardSpawner.SpawnCard("jolly joker", JokerArea)

            });

        PlayingCardArea.SetCards(new CardRenderer[]{

            PlayingCardSpawner.SpawnCard("as", "normal", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("2s", "normal", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("ad", "wild", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("2d", "normal", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("ah", "mult", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("kh", "normal", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("qc", "steel", PlayingCardArea),
            PlayingCardSpawner.SpawnCard("jc", "gold", PlayingCardArea)

            });

        CurrentBlind = ChipSpawner.SpawnChip("small blind", BlindLocation.localPosition);

        Module.OnActivate += delegate { StartCoroutine(DisplayBlindStartScreen(CurrentBlind)); };
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            RunTestCycle(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            RunTestCycle(true);
    }

    private void RunTestCycle(bool isRight)
    {
        var name = "";
        if (isRight)
            name = BlindData.FindNextBlind(CurrentBlind.GetInternalBlind().GetName());
        else
            name = BlindData.FindPrevBlind(CurrentBlind.GetInternalBlind().GetName());

        JokerArea.Deactivate();
        PlayingCardArea.Deactivate();
        CurrentBlind.Delete();

        CurrentBlind = ChipSpawner.SpawnChip(name, BlindLocation.localPosition);
        StartCoroutine(DisplayBlindStartScreen(CurrentBlind));
    }

    private IEnumerator DisplayBlindStartScreen(ChipRenderer blind)
    {
        yield return IntroScreen.Activate(blind, Audio);
        JokerArea.Activate();
        PlayingCardArea.Activate();
        StartCoroutine(IntroScreen.Deactivate(blind, BlindLocation.localPosition));
    }
}
