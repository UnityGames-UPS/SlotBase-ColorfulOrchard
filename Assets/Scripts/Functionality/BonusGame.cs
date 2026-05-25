using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Newtonsoft.Json;


public class BonusGame : MonoBehaviour
{

    [SerializeField] private Button[] btn;
    //[SerializeField] private Button btn2;
    //[SerializeField] private Button btn3;
    //[SerializeField] private Button btn4;
    //[SerializeField] private Button btn5;

    [SerializeField] private ImageAnimation[] imagelist;
    [SerializeField] private TMP_Text[] textList;
    [SerializeField] private Sprite[] GameOver;
    [SerializeField] private Sprite[] Symbol2;
    [SerializeField] private Sprite[] Symbol3;
    [SerializeField] private Sprite[] Symbol4;
    [SerializeField] private Sprite[] Symbol5;
    [SerializeField] private GameObject RayCast_Panel;

    //[SerializeField] private List<double> result = new List<double>();
    // [SerializeField] private List<Button> tempButtonList = new List<Button>();
    int counter = 0;
    [SerializeField] private GameObject bonusGame;
    [SerializeField] private SlotBehaviour slotBehaviour;
    [SerializeField] private AudioController audioManager;
    [SerializeField] private SocketIOManager SocketManager;
    List<int> randomIndex = new List<int>();
    internal bool WaitForBonusResult = true;

    void Start()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            int index = i;
            if (btn[index]) btn[index].onClick.RemoveAllListeners();
            if (btn[index]) btn[index].onClick.AddListener(delegate { OnSelectGrave(btn[index], imagelist[index], textList[index], index); });
        }
    }

    internal void StartBonusGame()
    {
        if (audioManager) audioManager.SwitchBGSound(true);
        if (RayCast_Panel) RayCast_Panel.SetActive(false);
        Initialize();
        bonusGame.SetActive(true);
        //result.Clear();
        //result = bonusResult;
        //Debug.Log("bonus result in bonus game: ," + JsonConvert.SerializeObject(result));
    }

    internal void resetgame()
    {
        if (audioManager) audioManager.SwitchBGSound(false);
        slotBehaviour.updateBalance();
        bonusGame.SetActive(false);
        slotBehaviour.CheckPopups = false;
    }

    private void Initialize()
    {
        randomIndex.Clear();
        counter = 0;

        foreach (var item in imagelist)
        {
            item.textureArray.Clear();
        }

        foreach (var item in btn)
        {
            item.interactable = true;
        }

        foreach (var item in textList)
        {
            item.transform.localPosition = Vector2.zero;
        }

        for (int i = 0; i < 4; i++)
        {
            randomIndex.Add(i);
        }
    }

    void OnSelectGrave(Button btn, ImageAnimation img, TMP_Text text, int graveNo)
    {
        if (RayCast_Panel) RayCast_Panel.SetActive(true);
        btn.interactable = false;
        StartCoroutine(DisplayBonusResult(btn, img, text, graveNo));
    }

    IEnumerator DisplayBonusResult(Button btn, ImageAnimation img, TMP_Text text, int graveNo)
    {
        int index = Random.Range(0, randomIndex.Count);

        WaitForBonusResult = true;
        SocketManager.OnBonusCollect(graveNo);
        yield return new WaitUntil(() => !WaitForBonusResult);

        if (SocketManager.bonusData.payload.payout == 0)
        {
            SocketManager.ResultData.payload.winAmount = SocketManager.bonusData.payload.winAmount;
            if (audioManager) audioManager.PlayBonusAudio("lose");
            PopulateAnimationSprites(img, -1);
            text.text = "GAME OVER";
            text.gameObject.SetActive(true);
            text.transform.DOLocalMoveY(140, 1f).onComplete = () =>
            {
                text.gameObject.SetActive(false);
            };
            img.StartAnimation();
            Invoke("resetgame", 2f);
            yield break;
        }
        if (audioManager) audioManager.PlayBonusAudio("win");
        PopulateAnimationSprites(img, randomIndex[index]);

        double value = SocketManager.bonusData.payload.winAmount;
        text.text = "+" + value.ToString("0.000");

        randomIndex.Remove(index);
        text.gameObject.SetActive(true);
        text.transform.DOLocalMoveY(140, 1f).onComplete = () =>
        {

            text.gameObject.SetActive(false);

        };

        img.StartAnimation();
        if (RayCast_Panel) RayCast_Panel.SetActive(false);
    }

    private void PopulateAnimationSprites(ImageAnimation animScript, int val)
    {
        switch (val)
        {
            case -1:
                for (int i = 0; i < GameOver.Length; i++)
                {
                    animScript.textureArray.Add(GameOver[i]);
                }
                break;
            case 0:
                for (int i = 0; i < Symbol2.Length; i++)
                {
                    animScript.textureArray.Add(Symbol2[i]);
                }
                break;
            case 1:
                for (int i = 0; i < Symbol3.Length; i++)
                {
                    animScript.textureArray.Add(Symbol3[i]);
                }
                break;
            case 2:
                for (int i = 0; i < Symbol4.Length; i++)
                {
                    animScript.textureArray.Add(Symbol4[i]);
                }
                break;
            case 3:
                for (int i = 0; i < Symbol5.Length; i++)
                {
                    animScript.textureArray.Add(Symbol5[i]);
                }
                break;
        }
    }

}
