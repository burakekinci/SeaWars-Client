using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {get; private set;}

    private void Awake() {
        if(Instance != null && Instance != this ){
            Destroy(this);
        }else{
            Instance = this;
        }
    }

    [SerializeField] private string _playerName = "Player1";
    public string PlayerName { 
        get{ return _playerName;} 
        private set{ _playerName = value;}
    }
    
    [SerializeField] ShipType _shipType = ShipType.CORVETTE;
    public ShipType shipType{
        get { return _shipType;}
    }

    [SerializeField] private float _localPlayerMoney = 3500f;
    public float PlayerMoney{ 
        get{ return _localPlayerMoney;} 
        private set{_localPlayerMoney = value;}
    }

    public void UpdateLocalPlayerMoney(float newMoney){
        _localPlayerMoney = newMoney;
    }


    private List<Ship> _localPlayerShips = new List<Ship>
    {
        new Ship{id=1,name="Light Ship",isBought=true},
        new Ship{id=2,name="Medium Ship",isBought=false},
        new Ship{id=3,name="Large Ship",isBought=true},
        new Ship{id=4,name="Heavy Ship",isBought=false}
    };

    public List<Ship> LocalPlayerShips {
        get{ return _localPlayerShips;}
        private set{_localPlayerShips = value;}
    }

    public void UpdateLocalPlayerShips(List<Ship> newShips){
        _localPlayerShips = newShips;
    }

    private void GetAPIData(){

    }

    public float GetUpdatedPlayerMoney(){
        GetAPIData();
        return _localPlayerMoney;
    }

    public List<Ship> GetUpdatedPlayerShips(){
        GetAPIData();
        return _localPlayerShips;
    }

}
