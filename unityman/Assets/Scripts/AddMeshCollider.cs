using UnityEngine;
using System.Collections;

public class AddMeshCollider : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        MeshCollider coll = GetComponent<MeshCollider>();
        if (coll == null)
        {
            coll = gameObject.AddComponent<MeshCollider>();
        }
        coll.sharedMesh = meshFilter.mesh;
	}
}
