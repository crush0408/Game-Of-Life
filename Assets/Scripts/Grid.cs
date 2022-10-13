using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject cellPrefab;
    public int width = 15;
    public int heigh = 15;
    public GameObject[,] objects;
    public bool [,] grids;
    bool [,] backGrids;
    public bool isPlaying;
    public GameObject PlayButton, PauseButton;

    IEnumerator playCoroutine;
    void Awake()
    {
        Canvas.GetDefaultCanvasMaterial().enableInstancing = true;
        Refresh();
        Stop();
    }

    public void InputWidth (string w)
	{
		width = int.Parse(w);
	}

	public void InputHeight (string height)
	{
		heigh = int.Parse(height);
	}
    public void Refresh(){
        Stop();
        foreach(Transform t in transform){
            Destroy(t.gameObject);
        }
        objects = new GameObject[width,heigh];
        grids = new bool[width,heigh];
        backGrids = new bool[width,heigh];

        var gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = width;
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x * width,gridLayout.cellSize.y * heigh);

        for(int i = 0;i < heigh; ++i){
            for(int j = 0; j < width; ++j){
                GameObject cell = Instantiate(cellPrefab,transform);
                Cell cellobj = cell.GetComponent<Cell>();
                cellobj.X = j;
                cellobj.Y = i;
                cellobj.isAlive = false;
                objects[i,j] = cell;
            }
        }
    }

    public void Stop(){
        if(playCoroutine!= null){
            StopCoroutine(playCoroutine);
        }
        isPlaying = false;
        PlayButton.GetComponent<Button>().enabled = true;
        PauseButton.GetComponent<Button>().enabled = false;
    }
    public void Play(){
        isPlaying = true;
        PlayButton.GetComponent<Button>().enabled = false;
        PauseButton.GetComponent<Button>().enabled = true;
        StartCoroutine(playCoroutine = UpdateRoutine());
    }
    IEnumerator UpdateRoutine(){
        while(true){
            for(int y = 0;y < heigh;++y){
                for(int x = 0; x < width;++x){
                    bool isAlive = grids[x,y];
                    int count =0;
                    for(int yy = -1; yy <= 1; ++yy){
                        for(int xx = -1; xx <= 1; ++xx){
                            if(xx == 0 && yy ==0){
                                continue;
                            }
                            count += GetValueFromGrid(grids,x + xx,y+ yy,width,heigh);
                        }
                    }

                    if(isAlive){
                        backGrids[x,y] = count == 2 || count ==3;
                    }
                    else{
                        backGrids[x,y] = count == 3;
                    }
                }
            }
            Swap();

            yield return new WaitForSeconds (0.125f);   
        }
    }
    int GetValueFromGrid(bool[,] grid,int x,int y,int width, int heigh){
        if(x < 0 || y<0 || x >= heigh || y >= heigh)
            return 0;
        return grid[x,y] ? 1: 0;
    }
    public void Swap(){
        for(int y = 0; y < heigh; ++y){
            for(int x = 0; x < width; ++x){
                objects[x,y].GetComponent<Cell>().isAlive = backGrids[x,y];
            }
        }

        bool[,] temp = grids;
        grids = backGrids;
        backGrids = temp;
    }
}
