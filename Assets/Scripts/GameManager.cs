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
        Debug.Log("one stopped spinning \t score: " + score);
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


    
}
