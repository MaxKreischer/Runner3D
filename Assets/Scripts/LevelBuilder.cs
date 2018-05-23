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
    public int MinCorridorLength;
    public int MaxCorridorLength;
    public int MinCorridorWidth;
    public int MaxCorridorWidth;
    public int MaxDoorsPerCorridor;
    public int MinDoorsPerCorridor;
    public int MaxRoomWidth;
    public int MinRoomWidth;
    public int complexity; //Complexity should determine the number of corridors until stop is set to true

    private List<Vector3> buildpositions = new List<Vector3>();
    private List<Vector3> builddirections = new List<Vector3>();
    private bool stop; //When stop is true we close every corridor we build and start no new corridors
    // Use this for initialization
    void Start () {
        
        stop = false;
        buildpositions.Add(new Vector3(startX, startY, startZ));
        builddirections.Add(new Vector3(1, 0, 0));
        BuildCorridor(ref buildpositions, ref builddirections, false);
        //This should build the whole level
        //--TODO
        /*while (buildpositions.Count > 0)
        {
            BuildCorridor(ref buildpositions, ref builddirections, stop);
            buildpositions.RemoveAt(0);
        }*/

	}
	
    //BuildCorridor will build a corridor of random depth and width and with a random number of doors
    void BuildCorridor(ref List<Vector3> buildpositions, ref List<Vector3> builddirections, bool deadend)
    {
        Vector3 buildposition = buildpositions[0];
        Vector3 builddirection = builddirections[0];
        int sizeX;              //The size of the corridor one the global X-axis
        int sizeY;              //The size of the corridor on the global Y-axis

        if(builddirection.x > 0)
        {
            sizeX = Random.Range(MinCorridorLength,MaxCorridorLength);
            sizeY = Random.Range(MinCorridorWidth, MaxCorridorWidth);
        }
        else
        {
            sizeX = Random.Range(MinCorridorWidth, MaxCorridorWidth);
            sizeY = Random.Range(MinCorridorLength, MaxCorridorLength);
        }
        //Its easier to save now then to calculate where we have to go in the end
        Vector3 endposition = buildposition + Vector3.Scale(builddirection, new Vector3(sizeX, sizeY,0));
        Vector3 enddirection = builddirection;

        int numOfDoors = Random.Range(MinDoorsPerCorridor, MaxDoorsPerCorridor);           
        Vector3[] doorpositions = new Vector3[numOfDoors];
        Vector3[] doordirections = new Vector3[numOfDoors];
        for (int d = 0; d < doorpositions.Length; d++)
        {
            float side = Mathf.Sign(Random.Range(-1, 1));
            //The doorpositions are random anywhere in the corridor. The side is assigns above, also random.
            doorpositions[d] = buildposition + new Vector3(0.5f,0,0) +  Vector3.Scale(builddirection, new Vector3((int)Random.Range(2, sizeX-1), (int)Random.Range(2, sizeY-1), 1)) + (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection)* Mathf.Min(sizeX, sizeY)*0.5f*side; ;
            doordirections[d] = (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection) * side;
            buildpositions.Add(doorpositions[d]);
            builddirections.Add(doordirections[d]);
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
        if (deadend)
        {
            buildposition = endposition + Vector3.Scale(new Vector3(sizeX / 2.0f, 0.5f, sizeY / 2.0f), new Vector3(Mathf.Sign(enddirection.x) * 1, 1, Mathf.Sign(enddirection.z) * 1) - enddirection);
            builddirection = enddirection - new Vector3(Mathf.Sign(enddirection.x) * 1, 0, Mathf.Sign(enddirection.z) * 1);
            buildposition += 0.5f * builddirection;
            for (int j = 1; j <= height; j++)
            {
                for (int i=1; i <= Mathf.Min(sizeX,sizeY); i++)
                {
                    Instantiate(wall, buildposition, Quaternion.Euler(0,90,0));
                    buildposition += builddirection;
                }
                buildposition += new Vector3(0, 1, 0);
                builddirection = builddirection * -1.0f;
                buildposition += builddirection;
            }
        }
        else
        {
            BuildCorner(endposition, enddirection, Mathf.Min(sizeX, sizeY));
        }
    }
    //BuildCorner will build a corner at the end of a corridor
    //--TODO-- Boden, Decke und andere Richtung
    void BuildCorner(Vector3 buildposition, Vector3 builddirection, int width)
    {
        buildposition += Vector3.Scale(new Vector3(width / 2.0f, 0.5f, width / 2.0f), new Vector3(Mathf.Sign(builddirection.x) * 1, 1, Mathf.Sign(builddirection.z) * 1) - builddirection);
        buildposition += 0.5f * builddirection;
        Vector3 startposition = buildposition;
        Vector3 startdirection = builddirection;
        for(int j = 1; j <= height; j++)
        {
            for(int i = 1; i <= width*2; i++)
            {
                if(i <= width)
                {
                    Instantiate(wall, buildposition, Quaternion.identity);
                    Instantiate(wall, buildposition - width * (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection), Quaternion.identity);
                }
                else
                {
                    Instantiate(wall, buildposition, Quaternion.identity);
                }
                buildposition += builddirection;
            }
            buildposition -= builddirection * 0.5f;
            builddirection = builddirection - new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1);
            buildposition += builddirection * 0.5f;
            for (int i = 1; i <= width * 2; i++)
            {
                if (i <= width)
                {
                    Instantiate(wall, buildposition, Quaternion.Euler(0,90,0));
                }
                else
                {
                    Instantiate(wall, buildposition - width * (new Vector3(Mathf.Sign(builddirection.x) * 1, 0, Mathf.Sign(builddirection.z) * 1) - builddirection), Quaternion.Euler(0, 90, 0));
                    Instantiate(wall, buildposition, Quaternion.Euler(0, 90, 0));
                }
                buildposition += builddirection;
            }
            buildposition = startposition + new Vector3(0, j, 0);
            builddirection = startdirection;
        }
    }
    //BuildRoom will build a room with random depth and width
    //--TODO--
    void BuildRoom(Vector3 buildposition, Vector3 builddirection)
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
