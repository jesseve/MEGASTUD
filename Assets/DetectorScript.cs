using UnityEngine;
using System.Collections.Generic;

public class DetectorScript : MonoBehaviour {

    public List<string> tagsToAttack;

    private Unit unit;

    void Start() {
        unit = transform.parent.GetComponent<Unit>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (tagsToAttack == null) return;
        if (tagsToAttack.Count == 0) return;
        if (tagsToAttack.Contains(other.tag) == true) {
            IDamageable dam = other.GetComponent<IDamageable>();
            if (dam != null)
                unit.StartAttack(dam);
        }
    }
}
