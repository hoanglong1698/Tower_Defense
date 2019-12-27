using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    public GameObject TowerPrefab { get => towerPrefab; }

    [SerializeField] private Sprite sprite;
    public Sprite Sprite { get => sprite; }

    [SerializeField] private int price;
    public int Price { get => price; }

    [SerializeField] private Text priceTxt;

    private void Start()
    {
        priceTxt.text = Price + " <color=yellow>$</color>";
    }
}
