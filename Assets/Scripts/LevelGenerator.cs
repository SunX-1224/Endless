using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour{

    Transform player;
    public List<GameObject> chunkTypes;

    public const int mapSize = 20;
    public const int chunkSize = 20;
    public const float maxViewDist = 150f;

    public static bool levelCompleted;
    public static Vector2 playerPosition;
    
    int endLine;
    int chunksInViewDist;
    Dictionary<Vector2, Chunk> chunksInWorld = new Dictionary<Vector2, Chunk>();
    HashSet<Vector2> lastFrameChunks = new HashSet<Vector2>();

    void Start(){
        levelCompleted = false;
        chunksInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);
        endLine = mapSize * chunkSize;
    }

    void Update(){
        playerPosition = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
        levelCompleted = playerPosition.y >= endLine;
    }

    void UpdateVisibleChunks(){
        int curChunkCoordX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int curChunkCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        foreach (Vector2 chunkCoord in lastFrameChunks){
            if(chunkCoord.y < (curChunkCoordY-2) ){
                chunksInWorld.Remove(chunkCoord, out Chunk c);
                c.DestroySelf();
            }
        }

        lastFrameChunks.Clear();

        for (int yOffset = -2; yOffset < chunksInViewDist; yOffset++){
            for (int xOffset = -chunksInViewDist; xOffset < chunksInViewDist; xOffset++){
                Vector2 chunkCoord = new(xOffset+curChunkCoordX, yOffset+curChunkCoordY); 

                if(!chunksInWorld.ContainsKey(chunkCoord)){
                    generateChunk(chunkCoord); 
                }
                lastFrameChunks.Add(chunkCoord);
            }
        }
    }

    int GetChunkID(Vector2 coord){
        if(coord.y > (mapSize - 1) || coord.y <= 3) return 0;

        coord.y -= 1f;
        if(!chunksInWorld.ContainsKey(coord)) return chunkTypes[0].GetComponent<ChunkData>().GetRandomNeighbour();
        
        return chunkTypes[chunksInWorld[coord].index].GetComponent<ChunkData>().GetRandomNeighbour();        
    }

    void generateChunk(Vector2 coord){
        int i = GetChunkID(coord);
        
        GameObject chunk = Instantiate(chunkTypes[i], this.transform);
        chunksInWorld.Add(coord, new Chunk(coord, chunkSize, chunk, i));
    }
    
    public void setPlayerTransform(Transform _player){
        player = _player;
    }
  
    public class Chunk{
        
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        public int index;

        public Chunk(Vector2 coord, int size, GameObject mesh, int i){
            index = i;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            meshObject = mesh;
            meshObject.transform.position = new Vector3(position.x, -0.5f, position.y);
        }
        
        public bool InRange(){
            float playerDist = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            return playerDist <= maxViewDist;
        }

        public void DestroySelf(){
            Destroy(meshObject);
        }
    }
} 


