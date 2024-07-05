using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//Each tile storing it's own information
public class TileInfo : MonoBehaviour
{   


    [SerializeField] Color baseColor1;
    [SerializeField] Color baseColor2;
    [SerializeField] Color hoverColor;
    internal int x , y ;
     TextMeshProUGUI text;

    // using this for tile coloring
    bool offTileReference;

    //Tile info for player and ai
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    MeshRenderer rnd;

    //List to store adjacent tiles to this tile
    public List<TileInfo> adjanceyTiles = new List<TileInfo>();

    //Breadth First Search
    public bool visited = false;
    public TileInfo parent = null;
    public int distance = 0;

    private void Start()
    {
        rnd = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if(current)
        {
            rnd.material.color = Color.magenta;
        }
        else if(target)
        {
            rnd.material.color = Color.yellow;
        }
        else if(selectable)
        {
            rnd.material.color = Color.cyan ;
        }
        else
        {
            rnd.material.color = offTileReference ? baseColor1 : baseColor2;
        }
    }
    public void SetColor(bool offTile, TextMeshProUGUI textUI)
    {
        text = textUI;
        offTileReference = offTile;
        GetComponent<MeshRenderer>().material.color = offTile ? baseColor1 : baseColor2;
    }
    public void SetTilePosition(int a,int b)
    {
        x = a; y = b;
    }

    //Hover thing
    private void OnMouseEnter()
    {
        rnd.material.color = hoverColor;
        text.text = x.ToString() + y.ToString();
    }

    private void OnMouseExit()
    {
        rnd.material.color = offTileReference ? baseColor1 : baseColor2;
    }


    internal void Reset()
    {
        adjanceyTiles.Clear();

      current = false;
      target = false;
     selectable = false;
  
     visited = false;
     parent = null;
     distance = 0;
}

    internal void FindNeighbour(float jumpHeight, TileInfo target)
    {
        Reset();
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(Vector3.forward,jumpHeight,target);
        CheckTile(-Vector3.forward,jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight,target);
    }

    void CheckTile(Vector3 direction, float jumpHeight, TileInfo target)
    {
        Vector3 halfExents = new Vector3(0.25f , (1 + jumpHeight)/2 , 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction,halfExents );

        foreach (Collider i in colliders) 
        {
          TileInfo tile = i.GetComponent<TileInfo>();
            if( tile != null && tile.walkable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 3) || tile == target){

                    adjanceyTiles.Add(tile);
                }
            }
        }
    }
}
