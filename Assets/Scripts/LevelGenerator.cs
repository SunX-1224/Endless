using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour{
    
    public Transform player;
    public Material groundmat;

    [SerializeField] Vector2 mapSize = new(10, 10);
    
    public const int chunkSize = 10;
    public const float maxViewDist = 60f;

    int chunksInViewDist;

    public static Vector2 playerPosition;
    
    Dictionary<Vector2, Chunk> chunkData = new Dictionary<Vector2, Chunk>();
    List<Vector2> lastFrameChunks = new List<Vector2>();
    void Start(){
        chunksInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);
    }

    void Update(){
        playerPosition = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks(){
        foreach (Vector2 chunkCoord in lastFrameChunks){
            chunkData[chunkCoord].SetVisible(false);    
        }
        lastFrameChunks.Clear();

        int curChunkCoordX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int curChunkCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for (int yOffset = 0; yOffset < chunksInViewDist; yOffset++){
            for (int xOffset = -chunksInViewDist; xOffset < chunksInViewDist; xOffset++){
                Vector2 chunkCoord = new(xOffset+curChunkCoordX, yOffset+curChunkCoordY); 

                if(chunkData.ContainsKey(chunkCoord)){
                    chunkData[chunkCoord].UpdateChunk();
                }else{
                    chunkData.Add(chunkCoord, new Chunk(chunkCoord, chunkSize, transform, groundmat));
                }
                lastFrameChunks.Add(chunkCoord);
            }
        }
    }


    public class Chunk{
        
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        public Chunk(Vector2 coord, int size, Transform parent, Material groundmat){
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.parent = parent;
            meshObject.transform.position = new Vector3(position.x, -1.0f, position.y);
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.GetComponent<MeshRenderer>().material = groundmat;

            SetVisible(false);
        }
        
        public void UpdateChunk(){
            float playerDist = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = playerDist <= maxViewDist;

            SetVisible(visible);
        }

        public void SetVisible(bool visibility){
            meshObject.SetActive(visibility);
        }

        public bool isVisible(){
            return meshObject.activeSelf;
        }
    }
}
