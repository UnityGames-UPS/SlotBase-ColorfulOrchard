using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BonusGame : MonoBehaviour
{
    [SerializeField]
    private GameObject BonusGame_Screen;
    [SerializeField]
    private TMP_Text Totalscore_text;
    [SerializeField]
    private TMP_Text times_text;
    [SerializeField]
    private List<OuterReelItem> SelectorItems;
    [SerializeField]
    private SocketIOManager socketManager;
    [SerializeField]
    private SlotBehaviour slotmanager;
    [SerializeField]
    private Button Lever_Button;
    [SerializeField]
    private SpineAnimController Lever_Anim;
    [SerializeField]
    private UIManager uIManager;

    private void Start()
    {
        Lever_Button.onClick.RemoveAllListeners();
        Lever_Button.onClick.AddListener(LeverHit);
    }

    internal void EndBonus()
    {
        BonusGame_Screen.SetActive(false);
        slotmanager.Balance_text.text = socketManager.BonusData.player.balance.ToString("f3");
        slotmanager.CheckPopups = false;
    }

    internal void StartBonus()
    {
        BonusGame_Screen.SetActive(true);
        currentIndex = 0;
        foreach (OuterReelItem p in SelectorItems)
        {
            p.selector.SetActive(false);
        }
        spinCount = socketManager.ResultData.payload.state.bonusSpinsLeft;
        times_text.text = spinCount.ToString();
        Totalscore_text.text = "0.00";
        Lever_Button.interactable = true;
    }

    private void LeverHit()
    {
        Debug.Log("hit the lever");
        Lever_Button.interactable = false;
        Lever_Anim.Stop();
        Lever_Anim.Play(false);
        socketManager.AccumulateResult(0);
        StartCoroutine(GameProcedure());
        spinCount--;
        times_text.text = spinCount.ToString();
    }
    private int currentIndex = 0;
    private int spinCount = 0;
    private IEnumerator GameProcedure()
    {
        int count = SelectorItems.Count;

        for (int loop = 0; loop < 2; loop++)
        {
            for (int step = 0; step < count; step++)
            {
                StepForward();
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }

        while (!socketManager.isResultdone)
        {
            StepForward();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (true)
        {
            StepForward();
            if (SelectorItems[currentIndex].id == socketManager.BonusData.payload.hitDetails.symbol)
            {
                times_text.text = socketManager.BonusData.payload.state.bonusSpinsLeft.ToString();
                Totalscore_text.text = socketManager.BonusData.payload.totalBonusWin.ToString();
                if (socketManager.BonusData.payload.isLastSpin)
                {
                    uIManager.FeatureEndSeq(socketManager.BonusData.payload.totalBonusWin.ToString());
                }
                else
                {
                    Lever_Button.interactable = true;
                }
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    private void StepForward()
    {
        int next = (currentIndex + 1) % SelectorItems.Count;
        SelectorItems[currentIndex].selector.SetActive(false);
        SelectorItems[next].selector.SetActive(true);
        currentIndex = next;
    }
}