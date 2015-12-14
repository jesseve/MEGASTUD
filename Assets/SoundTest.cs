//using UnityEngine;
//using System.Collections;
//
//public class SoundTest : MonoBehaviour {
//
//	AudioSource audioSource;
//	// Use this for initialization
//	void Start () {
//		audioSource = GetComponent<AudioSource>();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//		if(Input.GetKeyDown(KeyCode.A))
//		{
//			audioSource.clip = SoundManager.GetSoundClip(SoundClip.Attack);
//			audioSource.Play();
//		}
//
//		else if(Input.GetKeyDown(KeyCode.D))
//		{
//			audioSource.clip = SoundManager.GetSoundClip(SoundClip.Die);
//			audioSource.Play();			
//		}
//		else if(Input.GetKeyDown(KeyCode.U))
//		{
//			audioSource.clip = SoundManager.GetSoundClip(SoundClip.UnderAttack);
//			audioSource.Play();
//		}
//	}
//}
