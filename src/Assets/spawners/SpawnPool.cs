using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool
{
    // Constructor inputs
    private GameObject spawn_prefab;
    private Transform spawn_parent;

    // Pool-specific variables
    private List<GameObject> _pool;

    public SpawnPool(GameObject spawn_prefab, Transform spawn_parent, int initial_pool_size) {
        this.spawn_prefab = spawn_prefab;
        this.spawn_parent = spawn_parent;
        FillPool(initial_pool_size);
    }

    public void Remove(float z_cutoff) {

        /* Function which acts like normal object destroy, but only inactivates the object, so we can re-use it later */

        // Disable all inactive obstacles
        bool need_disable_item;
        foreach (GameObject pool_item in _pool) {
            need_disable_item = (pool_item.transform.position.z < z_cutoff);
            if (need_disable_item)
                pool_item.SetActive(false);
        }
    }

    public GameObject Create(Vector3 position, Quaternion rotation, Color color) {

        /* Function which acts like normal object instantiation, but pulls from pool instead of actually creating new objects */

        GameObject new_spawn = GetPoolItem();
        new_spawn.transform.position = position;
        new_spawn.transform.rotation = rotation;
        new_spawn.GetComponent<Renderer>().material.color = color;

        new_spawn.SetActive(true);

        return new_spawn;
    }


    private GameObject AddPoolItem() {

        /* Function which adds a single instantiated object to the pool */

        GameObject new_item = GameObject.Instantiate(spawn_prefab, spawn_parent);
        new_item.SetActive(false);
        _pool.Add(new_item);

        return new_item;
    }

    private GameObject ExtendPool(int num_extend) {

        /* Function used to add items to the pool after it's already been established */

        for (int i = 0; i < num_extend - 1; i++) {
            AddPoolItem();
        }
        GameObject new_item = AddPoolItem();
        Debug.LogWarningFormat("Extended spawn pool ({0} items)", _pool.Count);

        return new_item;
    }

    private void FillPool(int initial_count) {

        /* Function used to fill pool on startup */

        _pool = new List<GameObject>();
        for (int i = 0; i < initial_count; i++) {
            AddPoolItem();
        }
    }

    private GameObject GetPoolItem() {

        // Find first inactive item to return
        bool is_inactive;
        for (int i = 0; i < _pool.Count; i++) {
            is_inactive = !_pool[i].activeInHierarchy;
            if (is_inactive) {
                return _pool[i];
            }
        }

        // If we get here, we didn't find an inactive pool item! So instantiate more
        return ExtendPool(5);
    }


}
