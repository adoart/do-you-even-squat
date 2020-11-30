using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public List<GameObject> buildings;
    public float radius;
    public float checkRadius = 0.0f;
    private int layerMask;
    private readonly List<GameObject> buildingsPoll = new List<GameObject>();
    public int pollSize;

    // Start is called before the first frame update
    void Start() {
        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 8;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        SpawnBuildings();
    }

    // Update is called once per frame
    void Update() {
        SpawnBuildings();
        CleanUpBuildings();
        pollSize = buildingsPoll.Count;
    }

    public void DestroyBuilding(GameObject building) {
        building.SetActive(false);
    }

    private void SpawnBuildings() {
        foreach (GameObject building in buildings) {
            bool spawned = false;
            int tries = 0;
            while (!spawned && tries <= 3) {
                Vector2 position2D = Random.insideUnitCircle * radius;
                Vector3 position = new Vector3(transform.position.x + position2D.x, 15.0f, transform.position.z + position2D.y);
                float checkSphereRadius = checkRadius > 0.0f ? checkRadius : new Vector2(building.transform.localScale.x, building.transform.localScale.z).magnitude;
                if (!Physics.CheckSphere(position, checkSphereRadius, layerMask)) {
                    GameObject polledBuilding = buildingsPoll.Find(i => !i.activeSelf);
                    if (polledBuilding != null) {
                        polledBuilding.SetActive(true);
                        polledBuilding.transform.position = position;
                    } else {
                        buildingsPoll.Add(Instantiate(building, position, transform.localRotation));
                    }
                    spawned = true;
                } else {
                    tries++;
                }
            }
            
        }
    }

    private void CleanUpBuildings() {
        foreach (GameObject building in buildingsPoll) {
            float distance = (building.transform.position - transform.position).magnitude;
            if (distance > radius) {
                DestroyBuilding(building);
            }
        }
    }


}
