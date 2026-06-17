using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OuterReelItem : MonoBehaviour
{
    [SerializeField] internal int id;
    [SerializeField] internal GameObject selector;

    private void Awake()
    {
        selector = this.transform.GetChild(1).gameObject;
    }
}