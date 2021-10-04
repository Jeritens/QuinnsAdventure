using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int score;
    [SerializeField]
    Spawner spawner;

    [SerializeField]
    Player player;
    public List<SpinningTop> tops = new List<SpinningTop>();
    public float points;
    
    static public GameManager instance;
    public int extraLifes = 0;

    private void Awake() {
        instance = this;
        score = 0;
    }

    public void addSpinningTop(SpinningTop top){
        score++;
        tops.Add(top);
        //spawner.spawnSpinningTop();
    }

    public void stoppedSpinning(){
        if(extraLifes>0){
            extraLifes--;
            SpinAll(1000f);
        }
        else{
            Debug.Log("one stopped spinning \t score: " + score);
        }
        
    }
    public void SpinAll(float rpm){
        foreach (SpinningTop top in tops)
        {
            top.speedUp(rpm);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        foreach (SpinningTop top in tops)
        {
            points += top.pointsPerSecond *Time.deltaTime;
        }
    }
    public void AddPoints(float amount){
        points += amount;
    }


    
}
