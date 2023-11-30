using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData : MonoBehaviour {
    
    public List<int> neighbours;

    public int GetRandomNeighbour(int max = 6){
        if(neighbours.Count == 0) return Random.Range(0, max);
        return neighbours[Random.Range(0, neighbours.Count)];
    }
}
