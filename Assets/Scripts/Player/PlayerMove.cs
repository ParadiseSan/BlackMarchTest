using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Movement
{
    // Start is called before the first frame update
    void Start()
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
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
    }

    void CheckMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile") 
                { 
                  TileInfo t = hit.collider.GetComponent<TileInfo>();

                    if(t.selectable) // if tile can be selected to move then only execute further 
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
