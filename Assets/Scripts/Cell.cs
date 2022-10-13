using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X;
    public int Y;
    public bool isAlive;

    Grid grid;
    Image image;
    
    void Awake()
    {
        grid = transform.parent.GetComponent<Grid>();
        image = GetComponent<Image>();
    }

    
    void Update()
    {
        image.color = isAlive ? Color.blue : new Color(0.2f,0.2f,0.2f);
    }
    public void Toggle(){
        if(grid.isPlaying)
            return;
        isAlive = !isAlive;
        grid.grids[X,Y] = isAlive;
    }
}
