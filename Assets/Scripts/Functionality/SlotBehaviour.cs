using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

public class SlotBehaviour : MonoBehaviour
{
  [SerializeField]
  private RectTransform mainContainer_RT;

  [Header("Sprites")]
  [SerializeField]
  private Sprite[] myImages;


  [Header("Slot Images")]
  [SerializeField]
  private List<SlotImage> Tempimages;

  [Header("Slots Objects")]
  [SerializeField]
  private GameObject[] Slot_Objects;
  [Header("Slots Elements")]
  [SerializeField]
  private LayoutElement[] Slot_Elements;

  [Header("Slots Transforms")]
  [SerializeField]
  private Transform[] Slot_Transform;

  [Header("Buttons")]
  [SerializeField]
  private Button SlotStart_Button;
  [SerializeField] private Button AutoSpinStop_Button;
  [SerializeField]
  private Button AutoSpin_Button;
  [SerializeField]
  private Button MaxBet_Button;
  [SerializeField] private Button BetPerLine;
  [SerializeField] private Button Bet_plus;
  [SerializeField] private Button Bet_minus;
  [SerializeField] private Button StopSpin_Button;
  [SerializeField] private Button Turbo_Button;

  [Header("Turbo Animated Sprites")]
  [SerializeField] private Sprite[] TurboToggleSprites;

  [Header("Animated Sprites")]
  [SerializeField]
  private Sprite[] Symbol1;
  [SerializeField]
  private Sprite[] Symbol2;
  [SerializeField]
  private Sprite[] Symbol3;
  [SerializeField]
  private Sprite[] Symbol4;
  [SerializeField]
  private Sprite[] Symbol5;
  [SerializeField]
  private Sprite[] Symbol6;
  [SerializeField]
  private Sprite[] Symbol7;
  [SerializeField]
  private Sprite[] Symbol8;
  [SerializeField]
  private Sprite[] Symbol9;
  [SerializeField]
  private Sprite[] Symbol10;
  [SerializeField]
  private Sprite[] Symbol11;
  [SerializeField]
  private Sprite[] Symbol12;
  [SerializeField]
  private Sprite[] Symbol13;

  [Header("Miscellaneous UI")]
  [SerializeField]
  internal TMP_Text Balance_text;
  [SerializeField]
  private TMP_Text TotalBet_text;
  [SerializeField]
  internal TMP_Text TotalWin_text;
  [SerializeField]
  private TMP_Text LineBet_text;

  private Dictionary<int, string> y_string = new Dictionary<int, string>();

  int tweenHeight = 0;

  [SerializeField]
  private GameObject Image_Prefab;

  [SerializeField]
  private PayoutCalculation PayCalculator;

  [SerializeField]
  private List<ImageAnimation> TempList;

  private List<Tweener> alltweens = new List<Tweener>();

  [SerializeField]
  private int IconSizeFactor = 100;

  private int numberOfSlots = 5;

  [SerializeField]
  int verticalVisibility = 3;

  [SerializeField]
  private SocketIOManager SocketManager;

  [SerializeField]
  private Sprite[] Box_Sprites;

  [SerializeField]
  private AudioController audioController;

  [SerializeField]
  private UIManager uiManager;
  [SerializeField]
  private BonusGame _bonusManager;

  [SerializeField]
  private Button m_Gamble_Button;

  [Header("Free Spins Board")]
  [SerializeField]
  private GameObject FSBoard_Object;
  [SerializeField]
  private TMP_Text FSnum_text;

  internal int Lines = 20;

  Coroutine AutoSpinRoutine = null;
  private Coroutine FreeSpinRoutine = null;
  Coroutine tweenroutine;
  internal bool IsAutoSpin = false;
  internal bool IsSpinning = false;
  bool SlotRunning = false;
  private bool IsFreeSpin = false;
  internal bool CheckPopups = false;
  internal int BetCounter = 0;
  private double currentBalance = 0;
  internal double currentBet = 0;
  private double currentTotalBet = 0;
  internal bool IsHoldSpin = false;

  private bool StopSpinToggle;
  private bool IsTurboOn;
  internal bool WasAutoSpinOn;
  private float SpinDelay = 0.2f;
  private Tween ScoreTween;

  private bool CheckSpinAudio = false;

  private Tweener WinTween;
  private Sprite turboOriginalSprite;
  private int freeSpinsLeft;
  public static List<List<int>> initialGrid = new List<List<int>>()
    {
        new List<int>() { 8, 12, 7},
        new List<int>() { 12, 8, 7},
        new List<int>() { 12, 7, 11},
        new List<int>() { 12, 8, 7},
        new List<int>() { 8, 12, 7}
    };
  public static List<List<int>> initialAnimGrid = new List<List<int>>()
    {
        new List<int>() { 10, 10, 10},
        new List<int>() { 10, 10, 10},
        new List<int>() { 10, 10, 10},
        new List<int>() { 10, 10, 10},
        new List<int>() { 10, 10, 10}
    };
  private void Start()
  {
    if (SlotStart_Button) SlotStart_Button.onClick.RemoveAllListeners();
    if (SlotStart_Button) SlotStart_Button.onClick.AddListener(delegate { StartSlots(); });

    if (MaxBet_Button) MaxBet_Button.onClick.RemoveAllListeners();
    if (MaxBet_Button) MaxBet_Button.onClick.AddListener(MaxBet);

    if (AutoSpin_Button) AutoSpin_Button.onClick.RemoveAllListeners();
    if (AutoSpin_Button) AutoSpin_Button.onClick.AddListener(AutoSpin);

    if (AutoSpinStop_Button) AutoSpinStop_Button.onClick.RemoveAllListeners();
    if (AutoSpinStop_Button) AutoSpinStop_Button.onClick.AddListener(delegate { StopAutoSpin(); if (audioController) audioController.PlayButtonAudio(); });

    if (StopSpin_Button) StopSpin_Button.onClick.RemoveAllListeners();
    if (StopSpin_Button) StopSpin_Button.onClick.AddListener(() => { StopSpinToggle = true; StopSpin_Button.gameObject.SetActive(false); if (audioController) audioController.PlayButtonAudio(); });

    if (Turbo_Button) Turbo_Button.onClick.RemoveAllListeners();
    if (Turbo_Button) Turbo_Button.onClick.AddListener(delegate { TurboToggle(); if (audioController) audioController.PlayButtonAudio(); });

    if (Bet_plus) Bet_plus.onClick.RemoveAllListeners();
    if (Bet_plus) Bet_plus.onClick.AddListener(delegate { ChangeBet(true); });

    if (Bet_minus) Bet_minus.onClick.RemoveAllListeners();
    if (Bet_minus) Bet_minus.onClick.AddListener(delegate { ChangeBet(false); });

    tweenHeight = (13 * IconSizeFactor) - 500;
    if (FSBoard_Object) FSBoard_Object.SetActive(false);
    turboOriginalSprite = Turbo_Button.GetComponent<Image>().sprite;
    //TriggerPlusMinusButtons(0);
  }

  internal void AutoSpin()
  {
    if (!IsAutoSpin)
    {

      IsAutoSpin = true;


      if (AutoSpinStop_Button) AutoSpinStop_Button.gameObject.SetActive(true);
      // if (AutoSpin_Button) AutoSpin_Button.gameObject.SetActive(false);
      ToggleButtonGrp(false);
      if (AutoSpinRoutine != null)
      {
        StopCoroutine(AutoSpinRoutine);
        AutoSpinRoutine = null;
      }
      AutoSpinRoutine = StartCoroutine(AutoSpinCoroutine());
    }
  }
  void TurboToggle()
  {
    if (IsTurboOn)
    {
      IsTurboOn = false;
      Turbo_Button.GetComponent<ImageAnimation>().StopAnimation();
      Turbo_Button.image.sprite = turboOriginalSprite;
      //Turbo_Button.image.sprite = TurboToggleSprites[0];
      //Turbo_Button.image.color = new Color(0.86f, 0.86f, 0.86f, 1);
    }
    else
    {
      IsTurboOn = true;
      Turbo_Button.GetComponent<ImageAnimation>().StartAnimation();
      //Turbo_Button.image.color = new Color(1, 1, 1, 1);
    }
  }

  internal void shuffleInitialMatrix()
  {
    Debug.Log("Suffling------------------");
    for (int i = 0; i < Tempimages.Count; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        PopulateAnimationSprites(Tempimages[i].slotImages[j].transform.GetChild(0).GetComponent<ImageAnimation>(), initialGrid[i][j]);
        Tempimages[i].slotImages[j].transform.GetChild(0).GetComponent<Image>().sprite = myImages[initialGrid[i][j]];
      }
    }
    for (int i = 0; i < Tempimages.Count; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        Debug.Log(i + "--------" + initialAnimGrid[i][j] + "----------" + j);
        if (initialAnimGrid[i][j] > 0)
        {
          Debug.Log(initialAnimGrid[i][j]);
          StartGameAnimation(Tempimages[i].slotImages[j].gameObject);
        }
      }
    }
  }

  internal void FreeSpin(int spins)
  {
    if (!IsFreeSpin)
    {
      if (FSnum_text) FSnum_text.text = spins.ToString();
      if (FSBoard_Object) FSBoard_Object.SetActive(true);
      IsFreeSpin = true;
      ToggleButtonGrp(false);
      m_Gamble_Button.interactable = false;
      if (FreeSpinRoutine != null)
      {
        StopCoroutine(FreeSpinRoutine);
        FreeSpinRoutine = null;
      }
      FreeSpinRoutine = StartCoroutine(FreeSpinCoroutine(spins));

    }
  }

  private IEnumerator FreeSpinCoroutine(int spinchances)
  {
    int i = 0;
    while (i < spinchances)
    {
      i++;
      uiManager.FreeSpins--;
      StartSlots(IsAutoSpin);
      yield return tweenroutine;
      yield return new WaitForSeconds(SpinDelay);
      if (FSnum_text) FSnum_text.text = (spinchances - i).ToString();
    }
    freeSpinsLeft = 0;
    if (FSBoard_Object) FSBoard_Object.SetActive(false);

    if (WasAutoSpinOn)
    {
      AutoSpin();
    }
    else
    {
      ToggleButtonGrp(true);
    }
    IsFreeSpin = false;
  }

  private void ChangeBet(bool IncDec)
  {
    if (audioController) audioController.PlayButtonAudio();
    if (IncDec)
    {
      BetCounter++;
      if (BetCounter >= SocketManager.InitialData.bets.Count)
      {
        BetCounter = 0; // Loop back to the first bet
      }
    }
    else
    {
      BetCounter--;
      if (BetCounter < 0)
      {
        BetCounter = SocketManager.InitialData.bets.Count - 1; // Loop to the last bet
      }
    }
    uiManager.InitialiseUIData(SocketManager.UIData.paylines);
    Debug.Log("run this");
    if (LineBet_text) LineBet_text.text = SocketManager.InitialData.bets[BetCounter].ToString();
    if (TotalBet_text) TotalBet_text.text = (SocketManager.InitialData.bets[BetCounter] * Lines).ToString();
    currentTotalBet = SocketManager.InitialData.bets[BetCounter] * Lines;
    // CompareBalance();
  }

  private void TriggerPlusMinusButtons(int m_cmd)
  {
    switch (m_cmd)
    {
      case 0:
        Bet_plus.interactable = true;
        Bet_minus.interactable = false;
        break;
      case 1:
        Bet_plus.interactable = false;
        Bet_minus.interactable = true;
        break;
      case 2:
        Bet_plus.interactable = true;
        Bet_minus.interactable = true;
        break;
    }
  }

  private void CompareBalance()
  {
    if (currentBalance < currentTotalBet)
    {
      uiManager.LowBalPopup();
      // if (AutoSpin_Button) AutoSpin_Button.interactable = false;
      // if (SlotStart_Button) SlotStart_Button.interactable = false;
    }
    // else
    // {
    //     if (AutoSpin_Button) AutoSpin_Button.interactable = true;
    //     if (SlotStart_Button) SlotStart_Button.interactable = true;
    // }
  }

  private void StopAutoSpin()
  {
    if (IsAutoSpin)
    {
      IsAutoSpin = false;
      if (AutoSpinStop_Button) AutoSpinStop_Button.gameObject.SetActive(false);
      if (AutoSpin_Button) AutoSpin_Button.gameObject.SetActive(true);
      StartCoroutine(StopAutoSpinCoroutine());
    }

  }

  private IEnumerator AutoSpinCoroutine()
  {

    while (IsAutoSpin)
    {
      WasAutoSpinOn = true;
      StartSlots(IsAutoSpin);
      yield return tweenroutine;
      yield return new WaitForSeconds(SpinDelay);
    }
    WasAutoSpinOn = false;
  }

  internal void FetchLines(string LineVal, int count)
  {
    y_string.Add(count, LineVal);
  }

  private IEnumerator StopAutoSpinCoroutine()
  {
    yield return new WaitUntil(() => !IsSpinning);
    ToggleButtonGrp(true);


    if (AutoSpinRoutine != null || tweenroutine != null)
    {
      StopCoroutine(AutoSpinRoutine);
      StopCoroutine(tweenroutine);
      tweenroutine = null;
      AutoSpinRoutine = null;
      StopCoroutine(StopAutoSpinCoroutine());
    }
  }

  #region Hold Button To Start Auto Spin
  //Start Auto Spin on Button Hold

  internal void StartSpinRoutine()
  {
    if (!IsSpinning)
    {
      IsHoldSpin = false;
      Invoke("AutoSpinHold", 1.5f);
    }

  }

  internal void StopSpinRoutine()
  {
    CancelInvoke("AutoSpinHold");
    if (IsAutoSpin)
    {
      IsAutoSpin = false;
      if (AutoSpinStop_Button) AutoSpinStop_Button.gameObject.SetActive(false);
      //if (AutoSpin_Button) AutoSpin_Button.gameObject.SetActive(true);
      StartCoroutine(StopAutoSpinCoroutine());
    }
  }

  private void AutoSpinHold()
  {
    Debug.Log("Auto Spin Started");
    IsHoldSpin = true;
    AutoSpin();
  }
  #endregion

  internal void CallCloseSocket()
  {
    StartCoroutine(SocketManager.CloseSocket());
  }

  private void MaxBet()
  {
    if (audioController) audioController.PlayButtonAudio();

    if (TotalBet_text) TotalBet_text.text = "99999";
  }

  //internal void PopulateInitalSlots(int number, List<int> myvalues)
  //{
  //    PopulateSlot(myvalues, number);
  //}

  // internal void LayoutReset(int number)
  // {
  //   if (Slot_Elements[number]) Slot_Elements[number].ignoreLayout = true;
  //   if (SlotStart_Button) SlotStart_Button.interactable = true;
  // }

  //private void PopulateSlot(List<int> values, int number)
  //{
  //    if (Slot_Objects[number]) Slot_Objects[number].SetActive(true);
  //    for (int i = 0; i < values.Count; i++)
  //    {
  //        GameObject myImg = Instantiate(Image_Prefab, Slot_Transform[number]);
  //        images[number].slotImages.Add(myImg.transform.GetChild(0).GetComponent<Image>());
  //        images[number].slotImages[i].sprite = myImages[values[i]];
  //    }
  //    for (int k = 0; k < 2; k++)
  //    {
  //        GameObject mylastImg = Instantiate(Image_Prefab, Slot_Transform[number]);
  //        images[number].slotImages.Add(mylastImg.transform.GetChild(0).GetComponent<Image>());
  //        images[number].slotImages[images[number].slotImages.Count - 1].sprite = myImages[values[k]];
  //    }
  //    if (mainContainer_RT) LayoutRebuilder.ForceRebuildLayoutImmediate(mainContainer_RT);
  //    tweenHeight = (values.Count * IconSizeFactor) - 280;
  //    GenerateMatrix(number);
  //}

  private void PopulateAnimationSprites(ImageAnimation animScript, int val)
  {
    animScript.textureArray.Clear();
    animScript.textureArray.TrimExcess();
    switch (val)
    {
      case 0:
        for (int i = 0; i < Symbol1.Length; i++)
        {
          animScript.textureArray.Add(Symbol1[i]);
        }
        animScript.AnimationSpeed = Symbol1.Length - 7;
        break;
      case 1:
        for (int i = 0; i < Symbol2.Length; i++)
        {
          animScript.textureArray.Add(Symbol2[i]);
        }
        animScript.AnimationSpeed = Symbol2.Length - 7;
        break;
      case 2:
        for (int i = 0; i < Symbol3.Length; i++)
        {
          animScript.textureArray.Add(Symbol3[i]);
        }
        animScript.AnimationSpeed = Symbol3.Length - 7;
        break;
      case 3:
        for (int i = 0; i < Symbol4.Length; i++)
        {
          animScript.textureArray.Add(Symbol4[i]);
        }
        animScript.AnimationSpeed = Symbol4.Length - 7;
        break;
      case 4:
        for (int i = 0; i < Symbol5.Length; i++)
        {
          animScript.textureArray.Add(Symbol5[i]);
        }
        animScript.AnimationSpeed = Symbol5.Length - 7;
        break;
      case 5:
        for (int i = 0; i < Symbol6.Length; i++)
        {
          animScript.textureArray.Add(Symbol6[i]);
        }
        animScript.AnimationSpeed = Symbol6.Length - 7;
        break;
      case 6:
        for (int i = 0; i < Symbol7.Length; i++)
        {
          animScript.textureArray.Add(Symbol7[i]);
        }
        animScript.AnimationSpeed = Symbol7.Length - 7;
        break;
      case 7:
        for (int i = 0; i < Symbol8.Length; i++)
        {
          animScript.textureArray.Add(Symbol8[i]);
        }
        animScript.AnimationSpeed = Symbol8.Length - 7;
        break;
      case 8:
        for (int i = 0; i < Symbol9.Length; i++)
        {
          animScript.textureArray.Add(Symbol9[i]);
        }
        animScript.AnimationSpeed = Symbol9.Length - 7;
        break;
      case 9:
        for (int i = 0; i < Symbol10.Length; i++)
        {
          animScript.textureArray.Add(Symbol10[i]);
        }
        animScript.AnimationSpeed = Symbol10.Length - 7;
        break;
      case 10:
        for (int i = 0; i < Symbol11.Length; i++)
        {
          animScript.textureArray.Add(Symbol11[i]);
        }
        animScript.AnimationSpeed = Symbol11.Length - 7;
        break;
      case 11:
        for (int i = 0; i < Symbol12.Length; i++)
        {
          animScript.textureArray.Add(Symbol12[i]);
        }
        animScript.AnimationSpeed = Symbol12.Length - 7;
        break;
      case 12:
        for (int i = 0; i < Symbol13.Length; i++)
        {
          animScript.textureArray.Add(Symbol13[i]);
        }
        animScript.AnimationSpeed = Symbol13.Length - 7;
        break;
    }
  }

  private void StartSlots(bool autoSpin = false)
  {
    if (audioController) audioController.PlaySpinButtonAudio();
    if (!autoSpin)
    {
      if (AutoSpinRoutine != null)
      {
        StopCoroutine(AutoSpinRoutine);
        StopCoroutine(tweenroutine);
        tweenroutine = null;
        AutoSpinRoutine = null;
      }
    }
    if (TotalWin_text) TotalWin_text.text = "0.00";
    if (SlotAnimRoutine != null)
    {
      StopCoroutine(SlotAnimRoutine);
      SlotAnimRoutine = null;
    }

    // AnimStoppedProcess();
    if (SlotStart_Button) SlotStart_Button.interactable = false;
    if (TempList.Count > 0)
    {
      StopGameAnimation();
    }
    WinningsAnim(false);
    PayCalculator.ResetLines();
    // currentBalance = SocketManager.PlayerData.balance;
    tweenroutine = StartCoroutine(TweenRoutine());
  }

  private void OnApplicationFocus(bool focus)
  {
    audioController.CheckFocusFunction(focus, CheckSpinAudio);

  }

  [SerializeField]
  private List<int> TempLineIds;
  [SerializeField]
  private List<string> x_animationString;
  [SerializeField]
  private List<string> y_animationString;
  private IEnumerator TweenRoutine()
  {
    // currentBet = SocketManager.InitialData.bets[BetCounter] * SocketManager.InitialData.lines.Count;
    // currentTotalBet=SocketManager.InitialData.bets[BetCounter]*SocketManager.InitialData.lines.Count;
    if (currentBalance < currentTotalBet && !IsFreeSpin)
    {
      // CompareBalance();
      if (IsAutoSpin)
      {
        StopAutoSpin();
        yield return new WaitForSeconds(1);
      }
      ToggleButtonGrp(true);
      yield break;
    }
    if (audioController) audioController.PlayWLAudio("spin");
    IsSpinning = true;
    CheckSpinAudio = true;
    ToggleButtonGrp(false);

    if (!IsTurboOn && !IsFreeSpin && !IsAutoSpin)
    {
      StopSpin_Button.gameObject.SetActive(true);
    }

    for (int i = 0; i < numberOfSlots; i++)
    {
      InitializeTweening(Slot_Transform[i]);
      yield return new WaitForSeconds(0.1f);
    }

    if (!IsFreeSpin)
    {
      double bet = 0;
      double balance = 0;
      try
      {
        bet = double.Parse(TotalBet_text.text);
      }
      catch (Exception e)
      {
        Debug.Log("Error while conversion " + e.Message);
      }

      try
      {
        balance = double.Parse(Balance_text.text);
      }
      catch (Exception e)
      {
        Debug.Log("Error while conversion " + e.Message);
      }
      double initAmount = balance;

      balance = balance - bet;

      ScoreTween = DOTween.To(() => initAmount, (val) => initAmount = val, balance, 0.8f).OnUpdate(() =>
      {
        if (Balance_text) Balance_text.text = initAmount.ToString("f3");
      });
    }
    // SocketManager.AccumulateResult(BetCounter);

    // yield return new WaitUntil(() => SocketManager.isResultdone);

    yield return new WaitForSeconds(1f);
    // currentBalance = SocketManager.PlayerData.balance;

    // for (int i = 0; i < 3; i++)
    // {
    //   for (int j = 0; j < 5; j++)
    //   {
    //     int resultNum = int.Parse(SocketManager.ResultData.matrix[i][j]);
    //     //print("resultNum: " + resultNum);
    //     //print("image loc: " + j + " " + i);
    //     PopulateAnimationSprites(Tempimages[j].slotImages[i].transform.GetChild(0).GetComponent<ImageAnimation>(), resultNum);
    //     Tempimages[j].slotImages[i].transform.GetChild(0).GetComponent<Image>().sprite = myImages[resultNum];
    //   }
    // }

     yield return new WaitForSeconds(0.5f);

    if (IsTurboOn)                                                      // changes
    {

      yield return new WaitForSeconds(0.1f);
      StopSpinToggle = true;
    }
    else
    {
      for (int i = 0; i < 5; i++)
      {
        yield return new WaitForSeconds(0.1f);
        if (StopSpinToggle)
        {
          break;
        }
      }
      StopSpin_Button.gameObject.SetActive(false);
    }

    for (int i = 0; i < numberOfSlots; i++)
    {
      yield return StopTweening(Slot_Transform[i], i, StopSpinToggle);
    }

    StopSpinToggle = false;
    // yield return new WaitForSeconds(0.3f);

    yield return alltweens[^1].WaitForCompletion();

    // if (SocketManager.ResultData.payload.winAmount > 0)
    // {
    //   SpinDelay = 2f;
    // }
    // else
    // {
    //   SpinDelay = 0.2f;
    // }


    // if (SocketManager.ResultData.payload.winAmount > 0)
    // {
    //   List<int> winLine = new();
    //   foreach (var item in SocketManager.ResultData.payload.wins)
    //   {
    //     winLine.Add(item.line);
    //   }
    //   CheckPayoutLineBackend(winLine);
    //   //  if (m_Gamble_Button) m_Gamble_Button.interactable = true;
    // }
    // else
    // {
    //   if (audioController) audioController.StopWLAaudio();
    // }
    // CheckForFeaturesAnimation();
    KillAllTweens();

    ScoreTween?.Kill();
    // updateBalance();
    // CheckPopups = true;



    // if (SocketManager.ResultData.bonus.istriggered)
    // {

    //   StartBonus();
    // }
    // else
    // {
    //   CheckWinPopups();
    // }

    // yield return new WaitUntil(() => !CheckPopups);
    // if (SocketManager.ResultData.payload.winAmount > 0)
    //   WinningsAnim(true);


    // if (SocketManager.ResultData.freeSpin.isFreeSpin)
    // {
    //   if (IsAutoSpin)
    //   {
    //     StopAutoSpin();
    //     yield return new WaitForSeconds(0.1f);
    //   }
    //   if (IsFreeSpin)
    //   {
    //     IsFreeSpin = false;
    //     if (FreeSpinRoutine != null)
    //     {
    //       StopCoroutine(FreeSpinRoutine);
    //       FreeSpinRoutine = null;
    //     }
    //   }
    //   uiManager.FreeSpinProcess((int)SocketManager.ResultData.freeSpin.count);
    // }
    // if (!IsAutoSpin && !IsFreeSpin)
    // {
    //   ActivateGamble();
    //   ToggleButtonGrp(true);
    // }
      if (SlotStart_Button) SlotStart_Button.interactable = true;  //remove this just for testing
    IsSpinning = false;
    // else
    // {
    //     if (!IsFreeSpin)
    //     {
    //         ActivateGamble();
    //     }
    //     //  yield return new WaitForSeconds(1.5f);
    //     IsSpinning = false;
    // }

    //_bonusManager.startgame(new List<double> { 20, 50, 100, 120, 0 });
  }

  private void WinningsAnim(bool IsStart)
  {
    if (IsStart)
    {
      WinTween = TotalWin_text.transform.DOScale(new Vector2(1.2f, 1.2f), 1f).SetLoops(-1, LoopType.Yoyo).SetDelay(0);
    }
    else
    {
      WinTween.Kill();
      TotalWin_text.transform.localScale = Vector3.one;
    }
  }

  internal void CheckWinPopups()
  {
    if (SocketManager.ResultData.payload.winAmount >= currentTotalBet * 5 && SocketManager.ResultData.payload.winAmount < currentTotalBet * 10)
    {
      uiManager.PopulateWin(1, SocketManager.ResultData.payload.winAmount);
    }
    else if (SocketManager.ResultData.payload.winAmount >= currentTotalBet * 10 && SocketManager.ResultData.payload.winAmount < currentTotalBet * 15)
    {
      uiManager.PopulateWin(2, SocketManager.ResultData.payload.winAmount);
    }
    else if (SocketManager.ResultData.payload.winAmount >= currentTotalBet * 15)
    {
      uiManager.PopulateWin(3, SocketManager.ResultData.payload.winAmount);
    }
    else if (SocketManager.ResultData.scatter.amount > 0)
    {
      uiManager.PopulateWin(4, SocketManager.ResultData.payload.winAmount);
    }
    else
    {
      CheckPopups = false;
    }
  }

  internal void updateBalance()
  {
    if (Balance_text) Balance_text.text = SocketManager.PlayerData.balance.ToString("f3");
    if (TotalWin_text) TotalWin_text.text = SocketManager.ResultData.payload.winAmount.ToString("f3");
  }

  internal void StartBonus()
  {
    _bonusManager.StartBonusGame();
    if (SocketManager.ResultData.freeSpin.count > 0)
    {
      if (IsAutoSpin)
      {
        StopAutoSpin();
      }
    }
  }



  void ToggleButtonGrp(bool toggle)
  {

    if (SlotStart_Button) SlotStart_Button.interactable = toggle;
    if (MaxBet_Button) MaxBet_Button.interactable = toggle;
    if (AutoSpin_Button) AutoSpin_Button.interactable = toggle;
    if (BetPerLine) BetPerLine.interactable = toggle;
    // if(m_Gamble_Button) m_Gamble_Button.interactable = toggle;
    if (Bet_plus) Bet_plus.interactable = toggle;
    if (Bet_minus) Bet_minus.interactable = toggle;

    /*   if (toggle)
       {
           PlusMinusCheck();
       }
       else
       {
           if (Bet_plus) Bet_plus.interactable = toggle;
       } */

  }

  internal void SetInitialUI()
  {
    BetCounter = 0;
    //Debug.Log("run this");
    if (LineBet_text) LineBet_text.text = SocketManager.InitialData.bets[BetCounter].ToString();
    if (TotalBet_text) TotalBet_text.text = (SocketManager.InitialData.bets[BetCounter] * Lines).ToString();
    if (TotalWin_text) TotalWin_text.text = "0.00";
    if (Balance_text) Balance_text.text = SocketManager.PlayerData.balance.ToString("f2");
    currentBalance = SocketManager.PlayerData.balance;
    currentTotalBet = SocketManager.InitialData.bets[BetCounter] * Lines;
    CompareBalance();
    uiManager.InitialiseUIData(SocketManager.UIData.paylines);
  }

  private IEnumerator slotLineAnim()
  {
    int n = 0;
    if (IsAutoSpin || IsFreeSpin)
      yield break;
    PayCalculator.ResetLines();
    while (n < 5)
    {
      List<int> y_anim = null;
      for (int i = 0; i < TempLineIds.Count; i++)
      {
        y_anim = y_string[TempLineIds[i]]?.Split(',')?.Select(Int32.Parse)?.ToList();
        PayCalculator.GeneratePayoutLinesBackend(TempLineIds[i], true);
        for (int k = 0; k < y_anim.Count; k++)
        {
          if (Tempimages[k].slotImages[y_anim[k]].transform.GetChild(0).gameObject.GetComponent<ImageAnimation>().currentAnimationState == ImageAnimation.ImageState.PLAYING)
          {
            Tempimages[k].slotImages[y_anim[k]].transform.GetChild(0).gameObject.GetComponent<SlotScript>().SetBg(Box_Sprites[TempLineIds[i]]);
          }
        }
        yield return new WaitForSeconds(1.5f);
        for (int k = 0; k < y_anim.Count; k++)
        {
          if (Tempimages[k].slotImages[y_anim[k]].transform.GetChild(0).gameObject.GetComponent<ImageAnimation>().currentAnimationState == ImageAnimation.ImageState.PLAYING)
          {
            Tempimages[k].slotImages[y_anim[k]].transform.GetChild(0).gameObject.GetComponent<SlotScript>().ResetBG();
          }
        }
        PayCalculator.ResetStaticLine();
      }
      for (int i = 0; i < TempLineIds.Count; i++)
      {
        PayCalculator.GeneratePayoutLinesBackend(TempLineIds[i]);
      }
      yield return new WaitForSeconds(1.5f);
      PayCalculator.ResetLines();
      yield return new WaitForSeconds(1);
      n++;
    }
    AnimStoppedProcess();
  }

  private Coroutine SlotAnimRoutine = null;


  private void AnimStoppedProcess()
  {
    StopGameAnimation();
    for (int i = 0; i < Tempimages.Count; i++)
    {
      foreach (Image child in Tempimages[i].slotImages)
      {
        child.transform.GetChild(0).gameObject.GetComponent<SlotScript>().ResetBG();
      }
    }
    PayCalculator.ResetLines();
    TempLineIds.Clear();
    TempLineIds.TrimExcess();
  }

  private void StartGameAnimation(GameObject animObjects)
  {
    ImageAnimation temp = animObjects.transform.GetChild(0).GetComponent<ImageAnimation>();
    temp.StartAnimation();
    TempList.Add(temp);
  }

  private void StopGameAnimation()
  {
    for (int i = 0; i < TempList.Count; i++)
    {
      TempList[i].StopAnimation();
    }
    TempList.Clear();
    TempList.TrimExcess();
  }
  private void CheckForFeaturesAnimation()
  {
    bool playScatter = false;
    bool playBonus = false;
    bool playFreespin = false;
    if (SocketManager.ResultData.scatter.amount > 0)
    {
      playScatter = true;
    }
    if (SocketManager.ResultData.bonus.istriggered)
    {
      playBonus = true;
    }
    if (SocketManager.ResultData.freeSpin.isFreeSpin)
    {
      playFreespin = true;
    }
    PlayFeatureAnimation(playScatter, playBonus, playFreespin);
  }
  private void PlayFeatureAnimation(bool scatter = false, bool bonus = false, bool freeSpin = false)
  {
    for (int i = 0; i < SocketManager.ResultData.matrix.Count; i++)
    {
      for (int j = 0; j < SocketManager.ResultData.matrix[i].Count; j++)
      {

        if (int.TryParse(SocketManager.ResultData.matrix[i][j], out int parsedNumber))
        {
          if (scatter && parsedNumber == 12)
          {
            StartGameAnimation(Tempimages[j].slotImages[i].gameObject);
          }
          if (bonus && parsedNumber == 9)
          {
            StartGameAnimation(Tempimages[j].slotImages[i].gameObject);
          }
          if (freeSpin && parsedNumber == 10)
          {
            StartGameAnimation(Tempimages[j].slotImages[i].gameObject);
          }
        }

      }
    }
  }

  //generate the payout lines generated 
  private void CheckPayoutLineBackend(List<int> LineId, double jackpot = 0)
  {
    List<int> y_points = null;
    if (LineId.Count > 0)
    {
      if (jackpot <= 0)
      {
        if (audioController) audioController.PlayWLAudio("win");
      }

      for (int i = 0; i < LineId.Count; i++)
      {
        y_points = y_string[LineId[i]]?.Split(',')?.Select(Int32.Parse)?.ToList();
        PayCalculator.GeneratePayoutLinesBackend(LineId[i]);
      }

      if (jackpot > 0)
      {
        if (audioController) audioController.PlayWLAudio("megaWin");
        for (int i = 0; i < Tempimages.Count; i++)
        {
          for (int k = 0; k < Tempimages[i].slotImages.Count; k++)
          {
            StartGameAnimation(Tempimages[i].slotImages[k].gameObject);
          }
        }
      }
      else
      {
        List<KeyValuePair<int, int>> coords = new();
        for (int j = 0; j < LineId.Count; j++)
        {
          for (int k = 0; k < SocketManager.ResultData.payload.wins[j].positions.Count; k++)
          {
            int rowIndex = SocketManager.InitialData.lines[LineId[j]][k];
            int columnIndex = k;
            coords.Add(new KeyValuePair<int, int>(rowIndex, columnIndex));
          }
        }

        foreach (var coord in coords)
        {
          int rowIndex = coord.Key;
          int columnIndex = coord.Value;
          StartGameAnimation(Tempimages[columnIndex].slotImages[rowIndex].gameObject);
        }
      }
      WinningsAnim(true);
    }
    else
    {

      //if (audioController) audioController.PlayWLAudio("lose");
      if (audioController) audioController.StopWLAaudio();
    }
    CheckSpinAudio = false;
  }

  internal void GambleCollect()
  {
    SocketManager.OnCollect();                //hh
  }

  #region TweeningCode

  private void InitializeTweening(Transform slotTransform)
  {
    slotTransform.localPosition = new Vector2(slotTransform.localPosition.x, -IconSizeFactor);
    Tweener tweener = slotTransform.DOLocalMoveY(-tweenHeight, 0.4f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetDelay(0);
    tweener.Play();
    alltweens.Add(tweener);
  }
  private IEnumerator StopTweening(Transform slotTransform, int index, bool isStop)
  {
    alltweens[index].Pause();
    slotTransform.localPosition = new Vector2(slotTransform.localPosition.x, 0);
    alltweens[index] = slotTransform.DOLocalMoveY(-tweenHeight, 0.5f).SetEase(Ease.OutElastic,0.8f,0.5f);
    if (!isStop)
    {
      yield return new WaitForSeconds(0.2f);
    }
    else
    {
      yield return null;
    }
  }

  private void KillAllTweens()
  {
    for (int i = 0; i < numberOfSlots; i++)
    {
      alltweens[i].Kill();
    }
    alltweens.Clear();

  }
  #endregion
}

[Serializable]
public class SlotImage
{
  public List<Image> slotImages = new List<Image>(10);
}
