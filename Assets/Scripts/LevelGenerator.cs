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
    List<Vector2> lastFrameChunks = new List<Vector2>();

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
            if(chunkCoord.y < curChunkCoordY || !chunksInWorld[chunkCoord].InRange() ){
                chunksInWorld.Remove(chunkCoord, out Chunk c);
                c.DestroySelf();
            }else {
                chunksInWorld[chunkCoord].SetVisible(false);
            }
        }
        lastFrameChunks.Clear();

        for (int yOffset = 0; yOffset < chunksInViewDist; yOffset++){
            for (int xOffset = -chunksInViewDist; xOffset < chunksInViewDist; xOffset++){
                Vector2 chunkCoord = new(xOffset+curChunkCoordX, yOffset+curChunkCoordY); 

                if(chunksInWorld.ContainsKey(chunkCoord)){
                    chunksInWorld[chunkCoord].UpdateChunk();
                }else{
                    generateChunk(chunkCoord); 
                }
                lastFrameChunks.Add(chunkCoord);
            }
        }
    }

    void generateChunk(Vector2 coord){
        //level generation logic goes here
        int type;
        if(coord.y > (mapSize - 1) || coord.y <= 1)
            type = 0;
        else
            type = UnityEngine.Random.Range(0, chunkTypes.Count);

        // finally store the chunkData
        GameObject chunk = Instantiate(chunkTypes[type], this.transform);
        chunksInWorld.Add(coord, new Chunk(coord, chunkSize, chunk));
    }
    
    public void setPlayerTransform(Transform _player){
        player = _player;
    }
  
    public class Chunk{
        
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        public Chunk(Vector2 coord, int size, GameObject mesh){
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            meshObject = mesh;
            meshObject.transform.position = new Vector3(position.x, -0.5f, position.y);

            SetVisible(false);
        }
        
        public void UpdateChunk(){
            SetVisible(InRange());
        }

        public bool InRange(){
            float playerDist = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            return playerDist <= maxViewDist;
        }

        public void SetVisible(bool visibility){
            meshObject.SetActive(visibility);
        }

        public bool isVisible(){
            return meshObject.activeSelf;
        }

        public void DestroySelf(){
            Destroy(meshObject);
        }
    }
} 


