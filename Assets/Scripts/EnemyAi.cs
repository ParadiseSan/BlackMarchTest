using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAi : Movement
{
    
    GameObject target;
    private void Start()
    {
        Init();
    }
  
    // Update is called once per frame
    void Update()
    {
        if (!turn)
        {
            return;
        }
        if (!moving)
        {
            FindSelectableTiles();  // find the tiles on which the enemy can move like selectable one's
            FindNearestTarget();    // Find nearest target to approach
            CalculatePath();
                  
        }
        else
        {
            Move();
        }
    }

    void CalculatePath()
    {
       
          TileInfo targetTile = GetTargetTile(target);
         TileInfo nearestTile = NearestTile(targetTile.adjanceyTiles);
        

           MoveToTile(nearestTile);
               
    }

    void FindNearestTarget()
    {
        // any player unit 
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject t in targets)
        {
            float d = Vector3.Distance(transform.position,t.transform.position);

            if(d< distance) 
            {
               distance = d;
                nearest = t;
            }
        }

        target = nearest;
        
    }

    TileInfo NearestTile(List<TileInfo> adjanceyTiles)
    {
        TileInfo nearest = null;
        float distance = Mathf.Infinity;
        foreach (TileInfo tile in adjanceyTiles)
        {
            float d = Vector3.Distance(transform.position, tile.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = tile;
            }
        }
       // return nearest;

       return NearestSelectableTile(nearest);

    }

    TileInfo NearestSelectableTile(TileInfo targetTile)
    {
        TileInfo nearest = null;
        float distance = Mathf.Infinity;
        foreach (TileInfo tile in selectableTiles)
        {
            float d = Vector3.Distance(targetTile.transform.position,tile.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = tile;
            }
        }
        return nearest;
    }

}
