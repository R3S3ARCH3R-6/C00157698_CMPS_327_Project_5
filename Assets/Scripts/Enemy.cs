﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FSM States for the enemy
public enum EnemyState { STATIC, CHASE, REST, MOVING, DEFAULT };

public enum EnemyBehavior {EnemyBehavior1, EnemyBehavior2, EnemyBehavior3 };

public class Enemy : MonoBehaviour
{
    //pathfinding
    protected PathFinder pathFinder;
    public GenerateMap mapGenerator;
    protected Queue<Tile> path;
    protected GameObject playerGameObject;

    public Tile currentTile;
    protected Tile targetTile;
    public Vector3 velocity;

    //properties
    public float speed = 0.05f;
    public float visionDistance = 5;
    public int maxCounter = 5;
    protected int playerCloseCounter;

    protected EnemyState state = EnemyState.DEFAULT;
    protected Material material;

    public EnemyBehavior behavior = EnemyBehavior.EnemyBehavior1;

    //variables I added are below
    private float targetSpeed;
    //Tile playerTile;

    // Start is called before the first frame update
    void Start()
    {
        path = new Queue<Tile>();
        pathFinder = new PathFinder();
        playerGameObject = GameObject.FindWithTag("Player");
        playerCloseCounter = maxCounter;
        material = GetComponent<MeshRenderer>().material;

        targetSpeed = playerGameObject.GetComponent<Player>().fastSpeed;    //the speed of the player object
            //I added this code above and below (may not need it)
        //playerTile = playerGameObject.GetComponent<Player>().currentTile;   //gets the player's current tile
    }

    // Update is called once per frame
    void Update()
    {
        if (mapGenerator.state == MapState.DESTROYED) return;

        // Stop Moving the enemy if the player has reached the goal
        if (playerGameObject.GetComponent<Player>().IsGoalReached() || playerGameObject.GetComponent<Player>().IsPlayerDead())
        {
            //Debug.Log("Enemy stopped since the player has reached the goal or the player is dead");
            return;
        }

        switch(behavior)
        {
            case EnemyBehavior.EnemyBehavior1:  //random movement
                HandleEnemyBehavior1();
                //if the player is within a certain distance from the enemy, make the enemy chase the player
                if (Vector3.Distance(transform.position, playerGameObject.transform.position) <= visionDistance)
                {
                    behavior = EnemyBehavior.EnemyBehavior2;
                }
                break;
            case EnemyBehavior.EnemyBehavior2:
                HandleEnemyBehavior2();
                //if the player gets a certain distance away from the enemy, go back to random movement
                if (Vector3.Distance(transform.position, playerGameObject.transform.position) > visionDistance)
                {
                    behavior = EnemyBehavior.EnemyBehavior1;
                }
                break;
            case EnemyBehavior.EnemyBehavior3:
                HandleEnemyBehavior3();

                break;
            default:
                break;
        }

    }

    public void Reset()
    {
        Debug.Log("enemy reset");
        path.Clear();
        state = EnemyState.DEFAULT;
        currentTile = FindWalkableTile();
        transform.position = currentTile.transform.position;
    }

    Tile FindWalkableTile()
    {
        Tile newTarget = null;
        int randomIndex = 0;
        while (newTarget == null || !newTarget.mapTile.Walkable)
        {
            randomIndex = (int)(Random.value * mapGenerator.width * mapGenerator.height - 1);
            newTarget = GameObject.Find("MapGenerator").transform.GetChild(randomIndex).GetComponent<Tile>();
        }
        return newTarget;
    }

    // Dumb Enemy: Keeps Walking in Random direction, Will not chase player
    private void HandleEnemyBehavior1()
    {
        switch (state)
        {
            case EnemyState.DEFAULT: // generate random path 
                material.color = Color.white;
                if (path.Count <= 0) path = pathFinder.RandomPath(currentTile, 20);

                if (path.Count > 0)
                {
                    targetTile = path.Dequeue();
                    state = EnemyState.MOVING;
                }
                break;

            case EnemyState.MOVING:
                //move
                velocity = targetTile.gameObject.transform.position - transform.position;
                transform.position = transform.position + (velocity.normalized * speed);
                
                //if target reached
                if (Vector3.Distance(transform.position, targetTile.gameObject.transform.position) <= speed)
                {
                    currentTile = targetTile;
                    state = EnemyState.DEFAULT;
                }

                break;
            default:
                state = EnemyState.DEFAULT;
                break;
        }
    }

    // TODO: Enemy chases the player when it is nearby
    private void HandleEnemyBehavior2()
    {
        //the following code emulates a pursuit function
        /*if(Vector3.Distance(transform.position, playerGameObject.transform.position) <= visionDistance){

            float lookAheadTime = (transform.position - playerGameObject.transform.position).magnitude / (speed + targetSpeed);
            targetTile = playerGameObject.GetComponent<Player>().currentTile;   //gets the player's current tile
            velocity = targetTile.gameObject.transform.position - transform.position;   //gets the velocity of the target object/target location
            
            transform.position = transform.position + (velocity.normalized * lookAheadTime);

        }*/

        float lookAheadTime = (transform.position - playerGameObject.transform.position).magnitude / (speed + targetSpeed);
        targetTile = playerGameObject.GetComponent<Player>().currentTile;   //gets the player's current tile
        velocity = targetTile.gameObject.transform.position - transform.position;   //gets the velocity of the target object/target location

        transform.position += transform.position + (velocity.normalized * lookAheadTime);

    }

    // TODO: Third behavior (Describe what it does)
    private void HandleEnemyBehavior3()
    {
        pathFinder.FindPathAStar(currentTile, targetTile);
    }
}
