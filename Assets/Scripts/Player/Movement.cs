using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

//Base class for Movement and for finding tile
public class Movement : MonoBehaviour
{
    // the tiles on which player can move- highlighted in cyan during gameplay
    protected List<TileInfo> selectableTiles = new List<TileInfo>();

    // BFS path to the target
    Stack<TileInfo> path = new Stack<TileInfo>();
    TileInfo currentTile;
    //Tiles
    GameObject[] tiles;


    public bool moving = false; 
    public int move = 5;
    public float jumpHeight = 2f;
    public float moveSpeed = 2f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3(); //direction

    float halfHeight = 0;

    public bool turn = false;
    protected void Init()
    {
        // ineffecient but works for now
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        // half height is like to keep and move the player above the tiles and not w.r.t to the centre of the tile
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        
        // adding the character unit to the turn manager 
        TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public TileInfo GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        TileInfo tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<TileInfo>();
        }
        return tile;
    }

    public void ComputeAdjancyList(float jumpHeight, TileInfo target)
    {
        foreach (GameObject tile in tiles)
        {
            TileInfo t = tile.GetComponent<TileInfo>();
            t.FindNeighbour(jumpHeight , target);
        }
    }


    //BFS
    public void FindSelectableTiles()
    {
        ComputeAdjancyList(jumpHeight , null);
        GetCurrentTile();
        Queue<TileInfo> process = new Queue<TileInfo>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            TileInfo t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;


            if (t.distance < move)
            {
                foreach (TileInfo tile in t.adjanceyTiles)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(TileInfo tile) 
    {
        path.Clear();
        tile.target = true;
        moving = true;

        TileInfo next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    protected void Move()
    {
       if(path.Count >0)
        {
            TileInfo t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            
            if(Vector3.Distance(transform.position, target)>= 0.05f)
            {
                CalculateHeading(target);
                HorizontalVel();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }

        }
        else
        {
            RemoveSelectableTiles();
            moving = false; // movement is done

            TurnManager.EndTurn();
        }
    }

    void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (TileInfo tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    //heading is just the direction 
    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void HorizontalVel()
    {
        velocity = heading * moveSpeed;
    }

    // related to turn manager 
    public void BeginTurn()
    {
      
        turn = true;
    }
    public void EndTurn()
    {
        turn = false;
    }
}

