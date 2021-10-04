using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class ShopItem : MonoBehaviour
{
    public float price;
    public float multiply;
    [SerializeField]
    TextMeshPro text;
    Light lightAvailable;
    public float add;
    public bool oneTimeSell;
    public UnityEvent Event;
    void Start()
    {
        
        text = transform.parent.parent.GetComponentInChildren<TextMeshPro>();
        lightAvailable = transform.parent.parent.GetComponentInChildren<Light>();
        DisplayPrice();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        CheckPrice();
    }
    void CheckPrice(){
        bool affortable = GameManager.instance.points>=price;
        lightAvailable.enabled=affortable;
    }

    public bool buy(){ 
        Debug.Log("test");
        if(GameManager.instance.points >= price &&price>=0){
            
            GameManager.instance.AddPoints(-price);
            Event.Invoke();
            if(oneTimeSell){
                text.text="";
                price=-1;
                transform.parent.parent.GetChild(3).gameObject.SetActive(true);
            }
            else{
                IncreasePrice();
                DisplayPrice();
            }
            
            
            return true;
        }
        return false;
        
    }
    void IncreasePrice(){
        price += add;
        price *= multiply;
    }
    void DisplayPrice(){
        text.text = price.ToString();
        
    }
}
