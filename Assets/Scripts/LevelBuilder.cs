using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelBuilder : MonoBehaviour {

    public GameObject floor;
    public GameObject wall;
    public GameObject ceiling;

    public float startX;
    public float startY;
    public float startZ;
    public int height;
    Vector3 buildposition;
    Vector3 builddirection;
    bool stop;
    // Use this for initialization
    void Start () {
        buildposition = new Vector3(startX, startY, startZ);
        builddirection = new Vector3(1,0,0);
        stop = false;
        height = 4;
        BuildCorridor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //BuildCorridor will build a corridor of random depth and width and with a random number of doors
    void BuildCorridor()
    {

        int sizeX;              //The size of the corridor one the global X-axis
        int sizeY;              //The size of the corridor on the global Y-axis

        if(builddirection.x > 0)
        {
            sizeX = (int)Random.Range(10.0f,30.0f);
            sizeY = (int)Random.Range(2.0f, 4.0f);
        }
        else
        {
            sizeX = (int)Random.Range(2.0f, 4.0f);
            sizeY = (int)Random.Range(10.0f, 30.0f);
        }

        int numOfDoors = (int)Random.Range(1, 3);           
        Vector3[] doorpositions = new Vector3[numOfDoors];  
        for (int d = 0; d < doorpositions.Length; d++)
        {
            float side = Mathf.Sign(Random.Range(-1, 1));
            //The doorpositions are random anywhere in the corridor. The side is assigns above, also random.
            doorpositions[d] = buildposition + new Vector3(0.5f,0,0) +  Vector3.Scale(builddirection, new Vector3((int)Random.Range(2, sizeX-1), (int)Random.Range(2, sizeY-1), 1)) + (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection)* Mathf.Min(sizeX, sizeY)*0.5f*side; ;
        }
        //Instantiate the floor and the ceiling first
        GameObject f = Instantiate(floor, buildposition + builddirection * (Vector3.Dot(builddirection,new Vector3(sizeX, 1, sizeY)) / 2), Quaternion.Euler(90,0,0));
        f.transform.localScale = new Vector3(sizeX,sizeY,f.transform.localScale.z);
        GameObject c = Instantiate(ceiling, buildposition + builddirection * (Vector3.Dot(builddirection, new Vector3(sizeX, 1, sizeY)) / 2) + new Vector3(0,height,0), Quaternion.Euler(90, 0, 0));
        c.transform.localScale = new Vector3(sizeX, sizeY, c.transform.localScale.z);
        //Move the buildposition to one side of the corridor and to the "wall"
        buildposition += Vector3.Scale(new Vector3((float)sizeX /2.0f, 0.5f, (float)sizeY / 2.0f), new Vector3(Mathf.Sign(builddirection.x) * 1,1, Mathf.Sign(builddirection.z) * 1) - builddirection);
        buildposition += 0.5f * builddirection;

        //start building the walls
        for(int j = 1; j <= height; j++)
        {
            for (int i = 1; i <= Mathf.Max(sizeX, sizeY); i++)
            {
                //If we are in a doorposition, we build only the wall counterpart
                if(j <= 2 && InDoor(buildposition, doorpositions))
                {
                    Instantiate(wall, buildposition - Mathf.Min(sizeX, sizeY) * (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection), Quaternion.identity);
                }
                //If we are opposite to a door we build the wall only at the buildposition
                else if (j <= 2 && DoorCounterpart(buildposition, doorpositions, Mathf.Min(sizeX, sizeY)))
                {
                    Instantiate(wall, buildposition, Quaternion.identity);
                }
                //Otherwise we build at the buildposition and on the opposite
                else
                {
                    Instantiate(wall, buildposition, Quaternion.identity);
                    Instantiate(wall, buildposition - Mathf.Min(sizeX, sizeY) * (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection), Quaternion.identity);                   
                }
                buildposition += builddirection;
            }
            //Move buildposition one up and build in the opposite direction
            buildposition += new Vector3(0, 1, 0);
            builddirection = builddirection * -1.0f;
            buildposition += builddirection;
        }

    }
    //BuildRoom will build a room with random depth and width
    //--TODO--
    void BuildRoom()
    {
        float sizeX = Random.Range(1.0f, 30.0f);
        float sizeY = Random.Range(1.0f, 30.0f);
        GameObject f = Instantiate(floor,buildposition,Quaternion.identity);
        f.transform.localScale = new Vector3(sizeX, sizeY, f.transform.localScale.z);

    }
    //Check if the position is in a doorposition
    bool InDoor(Vector3 position, Vector3[] doorpositions)
    {
        for(int i=0; i < doorpositions.Length; i++)
        {
            if (position.x == doorpositions[i].x && position.z == doorpositions[i].z)
            {
                return true;
            }
        }
        return false;
    }
    //Check if the position on the opposite of a doorposition
    bool DoorCounterpart(Vector3 position, Vector3[] doorpositions, int size)
    {
        for (int i = 0; i < doorpositions.Length; i++)
        {
            if (position.x == doorpositions[i].x && position.z - size == doorpositions[i].z)
            {
                return true;
            }
        }
        return false;
    }
}
