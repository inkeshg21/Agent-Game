using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACOConnections
{
    private GameObject FromNode;
    private GameObject ToNode;
    private float distance;
    private float PheromoneLevel;
    private float PathProbability;
    
    public ACOConnections()
    {
    
    }

    public void SetConnection(GameObject FromNode, GameObject ToNode, float DefaultPheromoneLevel)
    {
        this.FromNode = FromNode;
        this.ToNode = ToNode;
        distance = Vector3.Distance(FromNode.transform.position, ToNode.transform.position);
        PheromoneLevel = DefaultPheromoneLevel;
        PathProbability = 0;
    }
    
    public GameObject GetFromNode()
    {
        return FromNode;
    }
    
    public GameObject GetToNode()
    {
        return ToNode;
    }
    public float GetDistance()
    {
        return distance;
    }
    
    public float GetPheromoneLevel()
    {
        return PheromoneLevel;
    }
 
    public void SetPheromoneLevel(float PheromoneLevel)
    {
        this.PheromoneLevel = PheromoneLevel;
    }
    
    public float GetPathProbability()
    {
        return PathProbability;
    }
    
    public void SetPathProbability(float PathProbability)
    {
        this.PathProbability = PathProbability;
    }
}
