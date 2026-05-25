using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{

  [Header("Menu UI")]
  [SerializeField]
  private Button Info_Button;

  [Header("Popus UI")]
  [SerializeField]
  private GameObject MainPopup_Object;

  [Header("info Popup")]
  [SerializeField]
  private GameObject PaytablePopup_Object;
  [SerializeField]
  private Button PaytableExit_Button;
  [SerializeField]
  private Button Next_Button;
  [SerializeField]
  private Button Previous_Button;
  private int paginationCounter = 1;
  [SerializeField] private GameObject[] PageList;
  [SerializeField] private Button[] paginationButtonGrp;
  [SerializeField] private Button Infoback_button;
  [SerializeField]
  private TMP_Text[] SymbolsText;
  [SerializeField]
  private TMP_Text FreeSpin_Text;
  [SerializeField]
  private TMP_Text Scatter_Text;

  [SerializeField] private TMP_Text Bonus_Text;

  [SerializeField] private TMP_Text Wild_Text;

  [Header("Settings Popup")]
  [SerializeField] private Button Setting_button;
  [SerializeField] private Button SettingExit_button;
  [SerializeField] private Button Setting_back_button;
  [SerializeField] private GameObject Setting_panel;
  [SerializeField] private Slider Sound_slider;
  [SerializeField] private Slider Music_slider;

  [Header("LowBalance Popup")]
  [SerializeField]
  private Button LBExit_Button;
  [SerializeField]
  private Button LBBack_Button;
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

  [Header("Megawin Popup")]
  [SerializeField] private GameObject megawin;
  [SerializeField] private TMP_Text megawin_text;
  [SerializeField] private Image Win_Image;
  [SerializeField] private Sprite HugeWin_Sprite;
  [SerializeField] private Sprite BigWin_Sprite;
  [SerializeField] private Sprite MegaWin_Sprite;
  [SerializeField] private Sprite Scater_Sprite;
  [SerializeField] private Button MegaWinHideBtn;

  [Header("FreeSpins Popup")]
  [SerializeField]
  private GameObject FreeSpinPopup_Object;
  [SerializeField]
  private TMP_Text Free_Text;
  [SerializeField]
  private Button FreeSpin_Button;

  //[Header("gamble game")]
  //[SerializeField] private Button Gamble_button;
  //[SerializeField] private Button GambleExit_button;
  //[SerializeField] private GameObject Gamble_game;

  [Header("Audio")]
  [SerializeField] private AudioController audioController;

  [SerializeField]
  private Button GameExit_Button;

  [SerializeField]
  private Button GameExitSplash_Button;

  [SerializeField]
  private Button GameExitBonus_Button;

  [SerializeField]
  private SlotBehaviour slotManager;

  [SerializeField]
  private SocketIOManager socketManager;

  private bool isExit = false;

  internal int FreeSpins;

  [SerializeField] internal GameObject RaycastBlocker;
  private void Start()
  {
    if (Info_Button) Info_Button.onClick.RemoveAllListeners();
    if (Info_Button) Info_Button.onClick.AddListener(delegate { paginationCounter = 1; GoToPage(0); OpenPopup(PaytablePopup_Object); });

    if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
    if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });

    if (Next_Button) Next_Button.onClick.RemoveAllListeners();
    if (Next_Button) Next_Button.onClick.AddListener(delegate { TurnPage(true); });

    if (Previous_Button) Previous_Button.onClick.RemoveAllListeners();
    if (Previous_Button) Previous_Button.onClick.AddListener(delegate { TurnPage(false); });

    if (Previous_Button) Previous_Button.interactable = false;

    if (paginationButtonGrp[0]) paginationButtonGrp[0].onClick.RemoveAllListeners();
    if (paginationButtonGrp[0]) paginationButtonGrp[0].onClick.AddListener(delegate { GoToPage(0); });

    if (paginationButtonGrp[1]) paginationButtonGrp[1].onClick.RemoveAllListeners();
    if (paginationButtonGrp[1]) paginationButtonGrp[1].onClick.AddListener(delegate { GoToPage(1); });

    if (paginationButtonGrp[2]) paginationButtonGrp[2].onClick.RemoveAllListeners();
    if (paginationButtonGrp[2]) paginationButtonGrp[2].onClick.AddListener(delegate { GoToPage(2); });

    if (paginationButtonGrp[3]) paginationButtonGrp[3].onClick.RemoveAllListeners();
    if (paginationButtonGrp[3]) paginationButtonGrp[3].onClick.AddListener(delegate { GoToPage(3); });

    if (Infoback_button) Infoback_button.onClick.RemoveAllListeners();
    if (Infoback_button) Infoback_button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });


    if (Setting_button) Setting_button.onClick.RemoveAllListeners();
    if (Setting_button) Setting_button.onClick.AddListener(delegate { OpenPopup(Setting_panel); });

    if (Sound_slider) Sound_slider.onValueChanged.RemoveAllListeners();
    if (Sound_slider) Sound_slider.onValueChanged.AddListener(delegate { ChangeSound(); });

    if (Music_slider) Music_slider.onValueChanged.RemoveAllListeners();
    if (Music_slider) Music_slider.onValueChanged.AddListener(delegate { ChangeMusic(); });

    if (FreeSpin_Button) FreeSpin_Button.onClick.RemoveAllListeners();
    if (FreeSpin_Button) FreeSpin_Button.onClick.AddListener(delegate { StartFreeSpins(FreeSpins); });

    if (SettingExit_button) SettingExit_button.onClick.RemoveAllListeners();
    if (SettingExit_button) SettingExit_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

    if (Setting_back_button) Setting_back_button.onClick.RemoveAllListeners();
    if (Setting_back_button) Setting_back_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

    if (MegaWinHideBtn) MegaWinHideBtn.onClick.RemoveAllListeners();
    if (MegaWinHideBtn) MegaWinHideBtn.onClick.AddListener(OnClickMegaWinHide);
    //if (Gamble_button) Gamble_button.onClick.RemoveAllListeners();
    //if (Gamble_button) Gamble_button.onClick.AddListener(delegate { OpenPopup(Gamble_game); });

    //if (GambleExit_button) GambleExit_button.onClick.RemoveAllListeners();
    //if (GambleExit_button) GambleExit_button.onClick.AddListener(delegate { ClosePopup(Gamble_game); });

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

    if (LBBack_Button) LBBack_Button.onClick.RemoveAllListeners();
    if (LBBack_Button) LBBack_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

    if (YesQuit_Button) YesQuit_Button.onClick.RemoveAllListeners();
    if (YesQuit_Button) YesQuit_Button.onClick.AddListener(CallOnExitFunction);

    if (CloseAD_Button) CloseAD_Button.onClick.RemoveAllListeners();
    if (CloseAD_Button) CloseAD_Button.onClick.AddListener(CallOnExitFunction);

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

    int ExtraSpins = spins - FreeSpins;
    FreeSpins = spins;


    if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(true);
    if (Free_Text) Free_Text.text = "You are awarded with " + ExtraSpins.ToString() + " extra free spins.";
    if (MainPopup_Object) MainPopup_Object.SetActive(true);
    DOVirtual.DelayedCall(2f, () =>
    {
      StartFreeSpins(spins);
    });
  }

  internal void PopulateWin(int type, double amount)
  {
    double initAmount = 0;
    double originalAmount = amount;
    switch (type)
    {
      case 1:
        if (Win_Image) Win_Image.sprite = BigWin_Sprite;
        break;
      case 2:
        if (Win_Image) Win_Image.sprite = HugeWin_Sprite;
        break;
      case 3:
        if (Win_Image) Win_Image.sprite = MegaWin_Sprite;
        break;
      case 4:
        if (Win_Image) Win_Image.sprite = Scater_Sprite;
        break;
    }
    if (megawin) megawin.SetActive(true);
    if (MainPopup_Object) MainPopup_Object.SetActive(true);

    DOTween.To(() => initAmount, (val) => initAmount = val, amount, 1f).OnUpdate(() =>
    {
      if (megawin_text) megawin_text.text = initAmount.ToString("f2");
    });

    DOVirtual.DelayedCall(3.5f, OnClickMegaWinHide);
  }

  private void OnClickMegaWinHide()
  {
    if (MainPopup_Object) MainPopup_Object.SetActive(false);
    if (megawin) megawin.SetActive(false);
    if (megawin_text) megawin_text.text = "0";
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
      if (paylines.symbols[i].name.ToUpper() == "FREESPIN")
      {
        if (FreeSpin_Text) FreeSpin_Text.text = paylines.symbols[i].description.ToString();
      }
      if (paylines.symbols[i].name.ToUpper() == "SCATTER")
      {
        if (Scatter_Text) Scatter_Text.text = paylines.symbols[i].description.ToString();
      }
      if (paylines.symbols[i].name.ToUpper() == "JACKPOT")
      {
        // if (Jackpot_Text) Jackpot_Text.text = paylines.symbols[i].description.ToString();
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

  private void TurnPage(bool type)
  {
    if (audioController) audioController.PlayButtonAudio();

    if (type)
      paginationCounter++;
    else
      paginationCounter--;


    GoToPage(paginationCounter - 1);


  }

  private void GoToPage(int index)
  {

    paginationCounter = index + 1;

    paginationCounter = Mathf.Clamp(paginationCounter, 1, 6);

    if (Next_Button) Next_Button.interactable = !(paginationCounter >= 6);

    if (Previous_Button) Previous_Button.interactable = !(paginationCounter <= 1);

    for (int i = 0; i < PageList.Length; i++)
    {
      PageList[i].SetActive(false);
    }

    for (int i = 0; i < paginationButtonGrp.Length; i++)
    {
      paginationButtonGrp[i].interactable = true;
      paginationButtonGrp[i].transform.GetChild(0).gameObject.SetActive(false);
    }

    PageList[paginationCounter - 1].SetActive(true);
    paginationButtonGrp[paginationCounter - 1].interactable = false;
    paginationButtonGrp[paginationCounter - 1].transform.GetChild(0).gameObject.SetActive(true);
  }

  private void ChangeSound()
  {
    audioController.ChangeVolume("wl", Sound_slider.value);
    audioController.ChangeVolume("button", Sound_slider.value);
  }

  private void ChangeMusic()
  {
    audioController.ChangeVolume("bg", Music_slider.value);

  }
}
