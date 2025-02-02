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
    public PlayField PlayField;
    public ChipRendererSpawner ChipSpawner;
    public Transform BlindLocation;
    public IntroScreen IntroScreen;
    public BackgroundAnimator BGAnim;
    public ColouredUISelectable MenuButton;

    private ChipRenderer CurrentBlind;

    void Awake()
    {
        _moduleID = _moduleIdCounter++;

        PlayField.SetJokers(new List<Joker>() {
            new Joker("chicot"),
            new Joker("hanging chad"),
            new Joker("photograph"),
            new Joker("square joker"),
            new Joker("golden ticket"),
        });
        PlayField.SetPlayingCards(new List<PlayingCard>() {
            new PlayingCard("as", "normal"),
            new PlayingCard("2s", "normal"),
            new PlayingCard("3s", "normal"),
            new PlayingCard("4s", "normal"),
            new PlayingCard("5s", "normal"),
            new PlayingCard("6s", "normal"),
            new PlayingCard("7s", "normal"),
            new PlayingCard("8s", "normal"),
        });

        CurrentBlind = ChipSpawner.SpawnChip("small blind", BlindLocation.localPosition);

        Module.OnActivate += delegate
        {
            BGAnim.SetColour(CurrentBlind.GetInternalBlind());
            StartCoroutine(DisplayBlindStartScreen(CurrentBlind));
        };

        MenuButton.OnRequestSound += PlayRequestedSound;
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

    private void PlayRequestedSound(string name, Transform trans)
    {
        Audio.PlaySoundAtTransform(name, trans);
    }

    private void RunTestCycle(bool isRight)
    {
        var name = "";
        if (isRight)
            name = BlindData.FindNextBlind(CurrentBlind.GetInternalBlind().GetName());
        else
            name = BlindData.FindPrevBlind(CurrentBlind.GetInternalBlind().GetName());

        PlayField.Deactivate();
        CurrentBlind.Delete();
        MenuButton.Deactivate();

        CurrentBlind = ChipSpawner.SpawnChip(name, BlindLocation.localPosition);
        StartCoroutine(DisplayBlindStartScreen(CurrentBlind));

        BGAnim.SetColour(CurrentBlind.GetInternalBlind());
    }

    private IEnumerator DisplayBlindStartScreen(ChipRenderer blind)
    {
        yield return IntroScreen.Activate(blind, Audio);
        PlayField.Activate();
        StartCoroutine(IntroScreen.Deactivate(blind, BlindLocation.localPosition));
        MenuButton.SetColour(Utility.LightenColour(blind.GetInternalBlind().GetBackgroundColour(), 0.75f));
        MenuButton.Activate();
    }
}
