using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeadHandler : MonoBehaviour {

    public GameObject prefab;

    private static DeadHandler dh;

    private List<DeadObject> free;
    private List<DeadObject> used;

    void Awake() {
        dh = this;
        CreatePool();
    }

    private void CreatePool() {

        used = new List<DeadObject>();
        free = new List<DeadObject>();

        for (int i = 0; i < 10; i++) {
            GameObject g = Instantiate(prefab) as GameObject;
            g.transform.SetParent(transform);
            free.Add(g.GetComponent<DeadObject>());
            g.SetActive(false);
        }
    }
    private DeadObject GetObj() {
        if (free.Count > 0)
        {
            DeadObject g = free[0];
            g.gameObject.SetActive(true);
            free.Remove(g);
            used.Add(g);

            return g;
        }
        else
        {
            DeadObject g = (Instantiate(prefab) as GameObject).GetComponent<DeadObject>();
            used.Add(g);
            return g;
        }
    }
    private void ReturnObj(DeadObject obj) {
        if (used.Contains(obj)) {
            used.Remove(obj);
            free.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public static void PlayAnimation(Vector3 location, Color primary, Color secondary, GameObject dead, float size = 1) {
        dh.GetObj().PlayAnimation(location, primary, secondary, dead, size);
    }

    public static void EndAnimation(DeadObject obj) {
        dh.ReturnObj(obj);
    }
}
