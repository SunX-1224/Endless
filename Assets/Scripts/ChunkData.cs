using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData : MonoBehaviour {
    
    public List<int> neighbours;

    public int GetRandomNeighbour(){
        return neighbours[Random.Range(0, neighbours.Count)];
    }
}
