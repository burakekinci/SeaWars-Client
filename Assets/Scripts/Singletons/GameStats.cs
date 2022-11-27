using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance {get; private set;}

    private void Awake() {
        if(Instance != null && Instance != this ){
            Destroy(this);
        }else{
            Instance = this;
        }
    }

    private Dictionary<int,float> _shipPrices = new Dictionary<int, float>{
        {1,1000},
        {2,1500},
        {3,2000},
        {4,2500}
    };

    public Dictionary<int,float> ShipPrices { 
        get{
            return _shipPrices;
        }
        private set{ _shipPrices = value;}
    }
}
