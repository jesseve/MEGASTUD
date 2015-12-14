using UnityEngine;
using System.Collections.Generic;

public enum SoundClip { 
	AttackOffensive1,
	AttackOffensive2,
	AttackOffensive3,
	AttackDefensive1,
	AttackDefensive2,
	AttackDefensive3,
	AttackDPS1,
	AttackDPS2, 
	AttackDPS3, 
	AttackTank1, 
	AttackTank2, 
	AttackTank3, 
	AttackScout1, 
	AttackScout2,
	AttackScout3,
	AttackTurret,
	AttackHQ,
	DieOffensive,
	DieDefensive,
	DieDPS,
	DieTank,
	DieScout,
	CollapseHQ, 
	CollapseTurret, 
	CollapseResource, 
	CollapseSpawner,
	PlaceBuilding,
	Alarm
}
public class SoundManager  {

	static private Dictionary<SoundClip, string> clipToName = new Dictionary<SoundClip, string>
	{
		{SoundClip.AttackOffensive1, "Attack"},
		{SoundClip.AttackOffensive2, "Attack"},
		{SoundClip.AttackOffensive3, "Attack"},
		{SoundClip.AttackDefensive1, "Attack"},
		{SoundClip.AttackDefensive2, "Attack"},
		{SoundClip.AttackDefensive3, "Attack"},
		{SoundClip.AttackDPS1,		 "Attack"},
		{SoundClip.AttackDPS2,		 "Attack"},
		{SoundClip.AttackDPS3,		 "Attack"},
		{SoundClip.AttackTank1,		 "Attack"},
		{SoundClip.AttackTank2,		 "Attack"},
		{SoundClip.AttackTank3,		 "Attack"},
		{SoundClip.AttackScout1, 	"Attack"},
		{SoundClip.AttackScout2, 	"Attack"},
		{SoundClip.AttackScout3, 	"Attack"},
		{SoundClip.AttackHQ, 		"Attack"},
		{SoundClip.AttackTurret, 	"Attack"},
		{SoundClip.DieOffensive, 	"Die"},
		{SoundClip.DieDefensive, 	"Die"},
		{SoundClip.DieDPS, 			"Die"},
		{SoundClip.DieTank, 		"Die"},
		{SoundClip.DieScout, 		"Die"},
		{SoundClip.CollapseHQ, 		"Die"},
		{SoundClip.CollapseTurret, 	"Die"},
		{SoundClip.CollapseResource,"Die"},
		{SoundClip.CollapseSpawner, "Die"},
		{SoundClip.PlaceBuilding, 	"Attack"},
		{SoundClip.Alarm, 			"UnderAttack"}
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
