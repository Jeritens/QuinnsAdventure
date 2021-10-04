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
    
    static public GameManager instance;

    private void Awake() {
        instance = this;
        score = 0;
    }

    public void addSpinningTop(){
        score++;
        spawner.spawnSpinningTop();
    }

    public void stoppedSpinning(){
        Debug.Log("one stopped spinning \t score: " + score);
    }


    
}
