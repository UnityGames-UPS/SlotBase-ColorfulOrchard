using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{

  [Header("Popus UI")]
  [SerializeField]
  private GameObject MainPopup_Object;

  [Header("info Popup")]
  [SerializeField]
  private GameObject PaytablePopup_Object;
  [SerializeField]
  private Button PaytableExit_Button;
  [SerializeField]
  internal Button PaytableEntry_Button;
  [SerializeField]
  private Button Rules_Button;
  [SerializeField]
  private Button Paylines_Button;
  [SerializeField]
  private Button Paytable_Button;
  [SerializeField]
  private Button Bonus_Button;
  [SerializeField]
  private Button Jackpot_Button;
  [SerializeField]
  private TMP_Text[] Header_texts;
  [SerializeField]
  private GameObject[] HeaderSelection_Objects;
  [SerializeField]
  private GameObject[] PageList;
  [SerializeField]
  private TMP_Text[] SymbolsText;
  [SerializeField]
  private TMP_Text Scatter_Text;
  [SerializeField]
  private TMP_Text Jackpot_Text;

  [SerializeField] private TMP_Text Bonus_Text;

  [SerializeField] private TMP_Text Wild_Text;

  [Header("Settings Popup")]
  [SerializeField] private Button SoundOn_Button;
  [SerializeField] private Button SoundOff_Button;

  [Header("LowBalance Popup")]
  [SerializeField]
  private Button LBExit_Button;
  [SerializeField]
  private GameObject LBPopup_Object;

  [Header("Disconnection Popup")]
  [SerializeField]
  private Button CloseDisconnect_Button;
  [SerializeField]
  private GameObject DisconnectPopup_Object;

  [Header("Reconection Popup")]
  [SerializeField]
  private GameObject ReconectingPopup_Object;

  [Header("AnotherDevice Popup")]
  [SerializeField]
  private Button CloseAD_Button;
  [SerializeField]
  private GameObject ADPopup_Object;

  [Header("Quit Popup")]
  [SerializeField]
  private GameObject QuitPopup_Object;
  [SerializeField]
  private Button YesQuit_Button;
  [SerializeField]
  private Button NoQuit_Button;
  [SerializeField]
  private Button CrossQuit_Button;
  [SerializeField]
  private Button BackQuit_Button;

  [Header("Miscellanous Win Popup")]
  [SerializeField] private GameObject WinPopup_Object;
  [SerializeField] private GameObject NormalWinPopup_Object;
  [SerializeField] private GameObject BigWinPopup_Object;
  [SerializeField] private GameObject GreatWinPopup_Object;
  [SerializeField] private GameObject MegaWinPopup_Object;
  [SerializeField] private GameObject BonusPopup_Object;
  [SerializeField] private GameObject FeatureEndPopup_Object;
  [SerializeField] private SpineAnimController NormalWin_Anim;
  [SerializeField] private SpineAnimController BigWin_Anim;
  [SerializeField] private SpineAnimController GreatWin_Anim;
  [SerializeField] private SpineAnimController MegaWin_Anim;
  [SerializeField] private SpineAnimController BonusWin_Anim;
  [SerializeField] private SpineAnimController FeatureEnd_Anim;
  [SerializeField] private TMP_Text WinText_text;
  [SerializeField] private TMP_Text NormalWinText_text;
  [SerializeField] private TMP_Text FeatureEndText_text;
  [SerializeField] private Button WinHideBtn;
  [SerializeField] private Button FeatureEndBtn;

  [Header("FreeSpins Popup")]
  [SerializeField]
  private GameObject FreeSpinPopup_Object;
  [SerializeField]
  private SpineAnimController FreeSpinPopup_Animation;

  [Header("Audio")]
  [SerializeField] private AudioController audioController;
  [SerializeField] private GameObject bonusSounds;

  [SerializeField]
  private Button GameExit_Button;

  [SerializeField]
  private Button GameExitSplash_Button;

  [SerializeField]
  private Button GameExitBonus_Button;

  [SerializeField]
  private SlotBehaviour slotManager;
  [SerializeField]
  private BonusGame bonusManager;

  [SerializeField]
  private SocketIOManager socketManager;

  private bool isExit = false;

  internal int FreeSpins;

  [SerializeField] internal GameObject RaycastBlocker;
  private void Start()
  {

    if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
    if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate
    {
      slotManager.WinDescription_text.text = "GOOD LUCK!";
      ClosePopup(PaytablePopup_Object);
    });

    if (PaytableEntry_Button) PaytableEntry_Button.onClick.RemoveAllListeners();
    if (PaytableEntry_Button) PaytableEntry_Button.onClick.AddListener(delegate
    {
      slotManager.WinDescription_text.text = "WELCOME!";
      OpenPopup(PaytablePopup_Object);
    });

    if (Rules_Button) Rules_Button.onClick.RemoveAllListeners();
    if (Rules_Button) Rules_Button.onClick.AddListener(delegate { OpenPage(0); });

    if (Paylines_Button) Paylines_Button.onClick.RemoveAllListeners();
    if (Paylines_Button) Paylines_Button.onClick.AddListener(delegate { OpenPage(1); });

    if (Paytable_Button) Paytable_Button.onClick.RemoveAllListeners();
    if (Paytable_Button) Paytable_Button.onClick.AddListener(delegate { OpenPage(2); });

    if (Bonus_Button) Bonus_Button.onClick.RemoveAllListeners();
    if (Bonus_Button) Bonus_Button.onClick.AddListener(delegate { OpenPage(3); });

    if (Jackpot_Button) Jackpot_Button.onClick.RemoveAllListeners();
    if (Jackpot_Button) Jackpot_Button.onClick.AddListener(delegate { OpenPage(4); });

    if (SoundOn_Button) SoundOn_Button.onClick.RemoveAllListeners();
    if (SoundOn_Button) SoundOn_Button.onClick.AddListener(delegate
    {
      SoundOn_Button.gameObject.SetActive(false);
      SoundOff_Button.gameObject.SetActive(true);
      ChangeSound(false);
    });

    if (SoundOff_Button) SoundOff_Button.onClick.RemoveAllListeners();
    if (SoundOff_Button) SoundOff_Button.onClick.AddListener(delegate
    {
      SoundOn_Button.gameObject.SetActive(true);
      SoundOff_Button.gameObject.SetActive(false);
      ChangeSound(true);
    });

    if (WinHideBtn) WinHideBtn.onClick.RemoveAllListeners();
    if (WinHideBtn) WinHideBtn.onClick.AddListener(OnClickWinHide);

    if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
    if (GameExit_Button) GameExit_Button.onClick.AddListener(delegate { OpenPopup(QuitPopup_Object); });

    if (GameExitSplash_Button) GameExitSplash_Button.onClick.RemoveAllListeners();
    if (GameExitSplash_Button) GameExitSplash_Button.onClick.AddListener(delegate { if (!isExit) { OpenPopup(QuitPopup_Object); } });

    if (GameExitBonus_Button) GameExitBonus_Button.onClick.RemoveAllListeners();
    if (GameExitBonus_Button) GameExitBonus_Button.onClick.AddListener(delegate { if (!isExit) { OpenPopup(QuitPopup_Object); } });

    if (NoQuit_Button) NoQuit_Button.onClick.RemoveAllListeners();
    if (NoQuit_Button) NoQuit_Button.onClick.AddListener(delegate { ClosePopup(QuitPopup_Object); });

    if (CrossQuit_Button) CrossQuit_Button.onClick.RemoveAllListeners();
    if (CrossQuit_Button) CrossQuit_Button.onClick.AddListener(delegate { if (!isExit) { ClosePopup(QuitPopup_Object); } });

    if (BackQuit_Button) BackQuit_Button.onClick.RemoveAllListeners();
    if (BackQuit_Button) BackQuit_Button.onClick.AddListener(delegate { if (!isExit) { ClosePopup(QuitPopup_Object); } });

    if (LBExit_Button) LBExit_Button.onClick.RemoveAllListeners();
    if (LBExit_Button) LBExit_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

    if (YesQuit_Button) YesQuit_Button.onClick.RemoveAllListeners();
    if (YesQuit_Button) YesQuit_Button.onClick.AddListener(CallOnExitFunction);

    if (CloseAD_Button) CloseAD_Button.onClick.RemoveAllListeners();
    if (CloseAD_Button) CloseAD_Button.onClick.AddListener(CallOnExitFunction);

    if (FeatureEndBtn) FeatureEndBtn.onClick.RemoveAllListeners();
    if (FeatureEndBtn) FeatureEndBtn.onClick.AddListener(FeatureEndTakeSeq);

    if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.RemoveAllListeners();
    if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.AddListener(delegate { CallOnExitFunction(); socketManager.closeSocketReactnativeCall(); });

  }

  internal void LowBalPopup()
  {
    OpenPopup(LBPopup_Object);
  }

  internal void DisconnectionPopup()
  {
    if (!isExit)
    {
      OpenPopup(DisconnectPopup_Object);
    }
  }

  private void StartFreeSpins(int spins)
  {
    if (MainPopup_Object) MainPopup_Object.SetActive(false);
    if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(false);
    slotManager.FreeSpin(spins);
  }

  internal void FreeSpinProcess(int spins)
  {
    FreeSpins = spins;

    if (MainPopup_Object) MainPopup_Object.SetActive(true);
    if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(true);
    FreeSpinPopup_Animation.Stop();
    FreeSpinPopup_Animation.Play(false);
    DOVirtual.DelayedCall(6f, () =>
    {
      StartFreeSpins(spins);
    });
  }

  internal void PopulateWin(int type, double amount)
  {
    double initAmount = 0;
    double originalAmount = amount;
    if (WinPopup_Object) WinPopup_Object.SetActive(true);
    if (MainPopup_Object) MainPopup_Object.SetActive(true);
    switch (type)
    {
      case 1:
        BigWinPopup_Object.SetActive(true);
        BigWin_Anim.Stop();
        BigWin_Anim.Play(false);
        break;
      case 2:
        GreatWinPopup_Object.SetActive(true);
        GreatWin_Anim.Stop();
        GreatWin_Anim.Play(false);
        break;
      case 3:
        MegaWinPopup_Object.SetActive(true);
        MegaWin_Anim.Stop();
        MegaWin_Anim.Play(false);
        break;
    }

    DOTween.To(() => initAmount, (val) => initAmount = val, amount, 1f).OnUpdate(() =>
    {
      if (WinText_text) WinText_text.text = initAmount.ToString("f2");
    });

    DOVirtual.DelayedCall(3f, OnClickWinHide);
  }

  internal void NormalWin(double amount)
  {
    NormalWinText_text.gameObject.SetActive(true);
    NormalWin_Anim.Stop();
    NormalWin_Anim.Play(false);
    NormalWinText_text.text = amount.ToString("f2");
    slotManager.CheckPopups = false;
    DOVirtual.DelayedCall(3f, () =>
    {
      NormalWinText_text.gameObject.SetActive(false);
    });
  }

  internal void BonusWinStartSequence()
  {
    BonusWin_Anim.Stop();
    if (MainPopup_Object) MainPopup_Object.SetActive(true);
    if (BonusPopup_Object) BonusPopup_Object.SetActive(true);
    BonusWin_Anim.Play(false);
  }
  internal void BonusWinEndSequence()
  {
    if (BonusPopup_Object) BonusPopup_Object.SetActive(false);
    if (MainPopup_Object) MainPopup_Object.SetActive(false);
  }
  internal void FeatureEndSeq(string amount)
  {
    if(audioController)audioController.PlayWLAudio("pinkwin");
    if (FeatureEndPopup_Object) FeatureEndPopup_Object.SetActive(true);
    if (MainPopup_Object) MainPopup_Object.SetActive(true);
    FeatureEndText_text.text = amount;
    FeatureEnd_Anim.Stop();
    FeatureEnd_Anim.Play(true);
    isEndGame = true;
    DOVirtual.DelayedCall(6f, () =>
    {
      if (isEndGame)
      {
        FeatureEndTakeSeq();
      }
    });
  }
  bool isEndGame = true;
  internal void FeatureEndTakeSeq()
  {
    isEndGame = false;
    if (FeatureEndPopup_Object) FeatureEndPopup_Object.SetActive(false);
    if (MainPopup_Object) MainPopup_Object.SetActive(false);
    if(audioController)audioController.StopWLAaudio();
    bonusManager.EndBonus();
  }
  private void OnClickWinHide()
  {
    if (MainPopup_Object) MainPopup_Object.SetActive(false);
    if (WinPopup_Object) WinPopup_Object.SetActive(false);
    if(audioController)audioController.StopWLAaudio();
    MegaWinPopup_Object.SetActive(false);
    BigWinPopup_Object.SetActive(false);
    GreatWinPopup_Object.SetActive(false);
    if (WinText_text) WinText_text.text = "0";
    slotManager.CheckPopups = false;
  }
  internal void ADfunction()
  {
    OpenPopup(ADPopup_Object);
  }

  private void CallOnExitFunction()
  {
    isExit = true;
    audioController.PlayButtonAudio();
    slotManager.CallCloseSocket();
    // Application.ExternalCall("window.parent.postMessage", "onExit", "*");
  }

  internal void InitialiseUIData(Paylines symbolsText)
  {
    PopulateSymbolsPayout(symbolsText);
  }


  private void PopulateSymbolsPayout(Paylines paylines)
  {
    double multiplyer = socketManager.InitialData.bets[slotManager.BetCounter];
    for (int i = 0; i < SymbolsText.Length; i++)
    {
      string text = null;
      if (paylines.symbols[i].multiplier[0] != 0)
      {
        text += "5x - " + paylines.symbols[i].multiplier[0] * multiplyer;
      }
      if (paylines.symbols[i].multiplier[1] != 0)
      {
        text += "\n4x - " + paylines.symbols[i].multiplier[1] * multiplyer;
      }
      if (paylines.symbols[i].multiplier[2] != 0)
      {
        text += "\n3x - " + paylines.symbols[i].multiplier[2] * multiplyer;
      }
      if (SymbolsText[i]) SymbolsText[i].text = text;
    }

    for (int i = 0; i < paylines.symbols.Count; i++)
    {
      if (paylines.symbols[i].name.ToUpper() == "SCATTER")
      {
        if (Scatter_Text) Scatter_Text.text = paylines.symbols[i].description.ToString();
      }
      if (paylines.symbols[i].name.ToUpper() == "JACKPOT")
      {
        if (Jackpot_Text) Jackpot_Text.text = paylines.symbols[i].description.ToString();
      }
      if (paylines.symbols[i].name.ToUpper() == "BONUS")
      {
        if (Bonus_Text) Bonus_Text.text = paylines.symbols[i].description.ToString();
      }
      if (paylines.symbols[i].name.ToUpper() == "WILD")
      {
        if (Wild_Text) Wild_Text.text = paylines.symbols[i].description.ToString();
      }
    }
  }
  internal void ReconnectionPopup()
  {
    OpenPopup(ReconectingPopup_Object);
  }

  private void OpenPopup(GameObject Popup)
  {
    if (audioController) audioController.PlayButtonAudio();
    if (Popup) Popup.SetActive(true);
    if (MainPopup_Object) MainPopup_Object.SetActive(true);
  }
  internal void CheckAndClosePopups()
  {
    if (ReconectingPopup_Object.activeInHierarchy)
    {
      ClosePopup(ReconectingPopup_Object);
    }
    if (DisconnectPopup_Object.activeInHierarchy)
    {
      ClosePopup(DisconnectPopup_Object);
    }
  }


  private void ClosePopup(GameObject Popup)
  {
    if (audioController) audioController.PlayButtonAudio();
    if (Popup) Popup.SetActive(false);
    if (!DisconnectPopup_Object.activeSelf)
    {
      if (MainPopup_Object) MainPopup_Object.SetActive(false);
    }
  }

  private void OpenPage(int counter)
  {
    foreach (GameObject p in PageList)
    {
      p.SetActive(false);
    }
    foreach (GameObject p in HeaderSelection_Objects)
    {
      p.SetActive(false);
    }
    foreach (TMP_Text p in Header_texts)
    {
      p.color = Color.white;
    }
    PageList[counter].SetActive(true);
    HeaderSelection_Objects[counter].SetActive(true);
    Header_texts[counter].color = Color.green;
  }

  private void ChangeSound(bool isOn)
  {
    audioController.gameObject.SetActive(isOn);
    bonusSounds.SetActive(isOn);
  }
}
