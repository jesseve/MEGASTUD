using UnityEngine;
using System.Collections.Generic;

public class SoundManager  {

	static private Dictionary<SoundClip, string> clipToName = new Dictionary<SoundClip, string>
	{
		{SoundClip.Attack, "Attack"},
		{SoundClip.Die, "Die"},
		{SoundClip.UnderAttack, "UnderAttack"}
	};


	private static AudioClip[] soundClips = new AudioClip[clipToName.Count]; 

	public static AudioClip GetSoundClip(SoundClip soundClip)
	{
		int index = (int)soundClip;
		if(soundClips[index] == null)
			soundClips[index] = Resources.Load("SoundClips/"+clipToName[soundClip], typeof(AudioClip)) as AudioClip;
		
		return soundClips[index];
	}
}
