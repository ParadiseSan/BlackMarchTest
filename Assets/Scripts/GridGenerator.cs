using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [SerializeField] int width = 10;
    [SerializeField] int height = 10;

    //Tile prefab
    [SerializeField] GameObject Tile;
    [SerializeField] Vector3 Offset;
    [SerializeField] GameObject TileParent;
    Camera cam;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        cam = Camera.main;
        Grid();
       
    }

    void Grid()
    {
        for(int i = 0;i< width; i++)
        {
            for (int j = 0; j< height; j++)
            {
                GameObject newTile = Instantiate(Tile , new Vector3(i,0,j) , Quaternion.identity);
                newTile.name = $"Tile {i} {j}";  
                newTile.GetComponent<TileInfo>().SetTilePosition(i, j);

                newTile.transform.parent = TileParent.transform;
                bool offTile = (i%2 ==0 && j%2 !=0 || i%2!=0 && j%2==0);
                newTile.GetComponent<TileInfo>().SetColor(offTile,text);
            }
        }

        cam.transform.position = Offset;
        cam.transform.rotation = Quaternion.Euler(50,0,0);
    }
}
