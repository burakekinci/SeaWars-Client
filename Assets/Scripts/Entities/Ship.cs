using System.Collections.Generic;

public class Ship : IEntity
{
    public int id {get; set;}
    public string name {get; set;}
    public bool isBought {get; set;}
    public bool isSelected {get; set;}
    public List<Upgrade> upgrades {get; set;}
}
