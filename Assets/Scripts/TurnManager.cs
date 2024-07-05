using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    static Dictionary<string , List <Movement>> units = new Dictionary<string , List<Movement>>(); //string is for tag each tag is a team

    static Queue<string> turnKey = new Queue<string>(); // current active team 

    static Queue <Movement> turnTeam = new Queue<Movement>(); // 

    // Update is called once per frame
    void Update()
    {
       
        if(turnTeam.Count == 0)
        {
            
            InitTeamTurnQueue();
        }
        
    }
    static void InitTeamTurnQueue()
    {
      
        List<Movement> teamList = units[turnKey.Peek()];

        foreach (Movement m in teamList)
        {
            turnTeam.Enqueue(m);
        }
        StartTurn();
        
    }

    public static void StartTurn()
    {
        if(turnTeam.Count >0) 
        {
            turnTeam.Peek().BeginTurn();
        }   
    }

    public static void EndTurn()
    {
        Movement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if(turnTeam.Count >0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Movement unit)
    {
        
        List<Movement> list;
        if (!units.ContainsKey(unit.tag))
        {
            
            list = new List<Movement>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
               
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
       
    }

    
}
