using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    public void Use(CharacterController user);
    
    public UsableType GetUsableType();
}

public enum UsableType
{
    Car,
    WeaponOnGround,
    AmmoBox
    //... TODO
}