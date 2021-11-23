using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    TowerButton towerBtnPressed;




    private void Update()
    {
        
    }


    public void SelectedTower(TowerButton towerSelected)
    {
        towerBtnPressed = towerSelected;
        Debug.Log("Pressed" + towerBtnPressed.gameObject);
    }
}
