using UnityEngine;
using System.Collections;

public class DeadObject : MonoBehaviour {

	public SpriteRenderer primaryColorSprite;
	public SpriteRenderer secondaryColorSprite;
    public Animator _anim;

    private GameObject objThatDied;

    public void PlayAnimation(Vector3 location, Color primary, Color secondary, GameObject died, float size = 1) {
        primaryColorSprite.color = primary;
        secondaryColorSprite.color = secondary;
        transform.localScale *= size;
        transform.position = location;
        objThatDied = died;
        _anim.Play("DiePoof");
    }
    public void End() {
        objThatDied.SetActive(false);
        DeadHandler.EndAnimation(this);
    }
}
