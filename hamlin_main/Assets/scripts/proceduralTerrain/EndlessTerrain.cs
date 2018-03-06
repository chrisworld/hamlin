﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour {

	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
	const float colliderGenerationDistanceThreshold = 5;

	public int colliderLODIndex;
	public LODInfo[] detailLevels;
	public static float maxViewDst;

	public Transform viewer;
	public Material mapMaterial;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static MapGenerator mapGenerator;
	int chunkSize;
	int chunksVisibleInViewDst;

  //set by MonsterManager itself
  [HideInInspector]
  public MonsterManager monsterManager;
  [HideInInspector]
  public Monster baseMonster;
  [HideInInspector]
  public int numMonstersPerChunk;
  [HideInInspector]
  public List<ScaleNames> scaleNames;
  [HideInInspector]
  public List<NoteBaseKey> baseKeys;

  Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	static List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

	void Start() {
		mapGenerator = FindObjectOfType<MapGenerator> ();
		maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = mapGenerator.mapChunkSize - 1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
		UpdateVisibleChunks ();
	}

	void Update() {
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

		if (viewerPosition != viewerPositionOld) {
			foreach (TerrainChunk chunk in visibleTerrainChunks) {
				chunk.UpdateCollisionMesh ();
			}
		}

    //only add monsters to current chunk, and only add if we have just changed chunk
    //TODO: for performance should probably remove old monsters stored in MonsterManager?
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();
      if(scaleNames.Count > 0 && baseKeys.Count > 0){
        GenerateMonsters();
      }
      else {
        print("not generating monsters yet as scale names and base keys not initialised");
        print(scaleNames.Count);
        print(baseKeys.Count);
      }
		}
	}
		
	void UpdateVisibleChunks() {
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2> ();
		for (int i = visibleTerrainChunks.Count-1; i >= 0; i--) {
			alreadyUpdatedChunkCoords.Add (visibleTerrainChunks [i].coord);
			visibleTerrainChunks [i].UpdateTerrainChunk ();
		}
			
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);




		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains (viewedChunkCoord)) {
					if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
						terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					} else {
						terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk (viewedChunkCoord, chunkSize, detailLevels, colliderLODIndex, transform, mapMaterial));
					}
				}

			}
		}
	}

  //helper method for addMonsters
  bool PositionIsValid(List<Vector2> invalidPositions, Vector2 position){

    int posX = Mathf.RoundToInt(position.x);
    int posY = Mathf.RoundToInt(position.y);
    bool result = true;

    foreach(Vector2 invalidPos in invalidPositions){
      if(Mathf.RoundToInt(invalidPos.x) == posX && Mathf.RoundToInt(invalidPos.y) == posY){
        result = false;
      }
    }
    
    return result;
  }

  //right now this only creates monsters within the viewer's current chunk
  public void GenerateMonsters(){

    //TODO: may need to get rid of this scaling as using for instantiating? idk. or swap y to z
    Vector3 viewerPos = new Vector3(viewer.position.x, viewer.position.y, viewer.position.z);

    //make the specified num of monsters
    for(int i = 0; i < numMonstersPerChunk; i++){

      //create random position within chunk, making sure it is not same position as player or existing monsters
      //NOTE: we assume no monsters are in the chunk when this method runs!!! when to call it to be sure??
      float offset = chunkSize / 2;
      Vector3 position = viewerPos;
      while (position.x == viewerPos.x && position.z == viewerPos.z) //in case by chance it is the same position
      {
        //DEBUG
        //position.x = position.x + Random.Range(0, 3);
        //position.z = position.z - Random.Range(0, 3);

        position.x = position.x + 0.5f;
        position.z = position.x + 0.5f;

        //need to adjust position.y to match height of mesh i.e. where ground is, can't just copy viewer.position.y
        //position.y = [HEIGHT OF MESH AT THESE X, Z COORDS]
        RaycastHit hit;
        //Ray ray = new Ray(new Vector3(x, maxHeight, y), Vector3.down);
        //somehow find out which chunk we are in
        //TODO: below should be run on specific chunk
        //if(TerrainChunk.meshCollider.Raycast(ray, out hit, 2.0f * maxHeight){
          //Debug.Log("Hit point: " + hit.point);
        //}

        //position.x = position.x - offset + Random.Range(0, chunkSize);
        //position.z = position.z + offset - Random.Range(0, chunkSize);
        print(position.x);
        print(position.z);
      }

      //TODO: maybe change this to be random rotation
      Quaternion rotation = Quaternion.identity;

      //create monster and add to MonsterManager's monsters list
      Monster monster = Instantiate<Monster>(baseMonster, position, rotation);
      monster.scale_name = scaleNames[ Random.Range(0, scaleNames.Count - 1) ];
      monster.base_key = baseKeys[ Random.Range(0, baseKeys.Count - 1) ];
      monster.gameObject.SetActive(true);
      monsterManager.monsters.Add(monster);
    }

  }


	public class TerrainChunk {

		public Vector2 coord;

		GameObject meshObject;
		Vector2 position;
		public Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;

		LODInfo[] detailLevels;
		LODMesh[] lodMeshes;
		int colliderLODIndex;

		public MapData mapData;
		bool mapDataReceived;
		int previousLODIndex = -1;
		bool hasSetCollider;

		public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Material material) {
			this.coord = coord;
			this.detailLevels = detailLevels;
			this.colliderLODIndex = colliderLODIndex;

			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshCollider = meshObject.AddComponent<MeshCollider>();
			meshRenderer.material = material;

			meshObject.transform.position = positionV3 * mapGenerator.terrainData.uniformScale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;
			SetVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++) {
				lodMeshes[i] = new LODMesh(detailLevels[i].lod);
				lodMeshes[i].updateCallback += UpdateTerrainChunk;
				if (i == colliderLODIndex) {
					lodMeshes[i].updateCallback += UpdateCollisionMesh;
				}
			}

			mapGenerator.RequestMapData(position,OnMapDataReceived);
		}

		void OnMapDataReceived(MapData mapData) {
			this.mapData = mapData;
			mapDataReceived = true;

			UpdateTerrainChunk ();
		}

		

		public void UpdateTerrainChunk() {
			if (mapDataReceived) {
				float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosition));

				bool wasVisible = IsVisible ();
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible) {
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++) {
						if (viewerDstFromNearestEdge > detailLevels [i].visibleDstThreshold) {
							lodIndex = i + 1;
						} else {
							break;
						}
					}

					if (lodIndex != previousLODIndex) {
						LODMesh lodMesh = lodMeshes [lodIndex];
						if (lodMesh.hasMesh) {
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						} else if (!lodMesh.hasRequestedMesh) {
							lodMesh.RequestMesh (mapData);
						}
					}


				}

				if (wasVisible != visible) {
					if (visible) {
						visibleTerrainChunks.Add (this);
					} else {
						visibleTerrainChunks.Remove (this);
					}
					SetVisible (visible);
				}
			}
		}

		public void UpdateCollisionMesh() {
			if (!hasSetCollider) {
				float sqrDstFromViewerToEdge = bounds.SqrDistance (viewerPosition);

				if (sqrDstFromViewerToEdge < detailLevels [colliderLODIndex].sqrVisibleDstThreshold) {
					if (!lodMeshes [colliderLODIndex].hasRequestedMesh) {
						lodMeshes [colliderLODIndex].RequestMesh (mapData);
					}
				}

				if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
					if (lodMeshes [colliderLODIndex].hasMesh) {
						meshCollider.sharedMesh = lodMeshes [colliderLODIndex].mesh;
						hasSetCollider = true;
					}
				}
			}
		}

		public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
		}

		public bool IsVisible() {
			return meshObject.activeSelf;
		}

	}

	class LODMesh {

		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		public event System.Action updateCallback;

		public LODMesh(int lod) {
			this.lod = lod;
		}

		void OnMeshDataReceived(MeshData meshData) {
			mesh = meshData.CreateMesh ();
			hasMesh = true;

			updateCallback ();
		}

		public void RequestMesh(MapData mapData) {
			hasRequestedMesh = true;
			mapGenerator.RequestMeshData (mapData, lod, OnMeshDataReceived);
		}

	}

	[System.Serializable]
	public struct LODInfo {
		[Range(0,MeshGenerator.numSupportedLODs-1)]
		public int lod;
		public float visibleDstThreshold;


		public float sqrVisibleDstThreshold {
			get {
				return visibleDstThreshold * visibleDstThreshold;
			}
		}
	}

}
