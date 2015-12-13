using UnityEngine;
using System.Collections;

public class DeadObject : MonoBehaviour {

	public SpriteRenderer primaryColorSprite;
	public SpriteRenderer secondaryColorSprite;
    public Animator _anim;


    public void PlayAnimation(Color primary, Color secondary, float size = 1) {
        primaryColorSprite.color = primary;
        secondaryColorSprite.color = secondary;
        transform.localScale *= size;
        _anim.Play("DiePoof");
    }
    public void End() {
        DeadHandler.EndAnimation(this);
    }
}
