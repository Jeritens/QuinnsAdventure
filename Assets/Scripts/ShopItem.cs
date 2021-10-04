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
    Light light;
    public float add;
    public bool oneTimeSell;
    public UnityEvent Event;
    public string name;
    void Start()
    {
        
        text = transform.parent.parent.GetComponentInChildren<TextMeshPro>();
        light = transform.parent.parent.GetComponentInChildren<Light>();
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
        if(price<0){
            light.enabled = false;
            return;
        }
        bool affortable = GameManager.instance.points>=price;
        light.enabled=affortable;
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
        text.text = name + "\n"+price.ToString();
        
    }
}
