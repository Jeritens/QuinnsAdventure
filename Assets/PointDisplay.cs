using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;
    [SerializeField]
    TextMeshProUGUI lifes;


    // Update is called once per frame
    void Update()
    {
        display.text = GameManager.instance.points.ToString("0");
        lifes.text = "x" + GameManager.instance.extraLifes.ToString("0");
    }
}
