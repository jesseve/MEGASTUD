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
        transform.localScale = Vector3.one * size;
        transform.position = location;
        objThatDied = died;
        _anim.Play("DiePoof");
    }

    public void End() {
		if(objThatDied != null && objThatDied.activeSelf == true)
		{
			if(objThatDied.activeInHierarchy)
			{
				BuildingBase building = objThatDied.GetComponent<BuildingBase>();
	        	
				if(building == null)
					objThatDied.SetActive(false);
				else
				{
					building.RespawnMe();
				}
			}
		}
        DeadHandler.EndAnimation(this);
    }
}
