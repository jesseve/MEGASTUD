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
		{SoundClip.AttackOffensive1, "Offensive1Attack"},
		{SoundClip.AttackOffensive2, "Offensive2Attack"},
		{SoundClip.AttackOffensive3, "Offensive3Attack"},
		{SoundClip.AttackDefensive1, "Defensive1Attack"},
		{SoundClip.AttackDefensive2, "Defensive2Attack"},
		{SoundClip.AttackDefensive3, "Defensive3Attack"},
		{SoundClip.AttackDPS1,		 "DPS1Attack"},   
		{SoundClip.AttackDPS2,		 "DPS2Attack"},   
		{SoundClip.AttackDPS3,		 "DPS3Attack"},   
		{SoundClip.AttackTank1,		 "TankAttack1"},
		{SoundClip.AttackTank2,		 "TankAttack2"},
		{SoundClip.AttackTank3,		 "TankAttack3"},
		{SoundClip.AttackScout1, 	 "ScoutAttack1"},
		{SoundClip.AttackScout2, 	 "ScoutAttack2"},
		{SoundClip.AttackScout3, 	 "ScoutAttack3"},
		{SoundClip.AttackHQ, 		 "AttackHQ"},
		{SoundClip.AttackTurret, 	 "AttackTurret"},
		{SoundClip.DieOffensive, 	 "DieOffensive"},
		{SoundClip.DieDefensive, 	 "DieDefensive"},
		{SoundClip.DieDPS, 			 "DieDPS"},
		{SoundClip.DieTank, 		 "DieTank"},
		{SoundClip.DieScout, 		 "DieScout"},
		{SoundClip.CollapseHQ, 		 "CollapseHQ"},
		{SoundClip.CollapseTurret, 	 "CollapseTurret"},
		{SoundClip.CollapseResource, "CollapseResource"},
		{SoundClip.CollapseSpawner,  "CollapseSpawner"},
		{SoundClip.PlaceBuilding, 	 "PlaceBuilding"},
		{SoundClip.Alarm, 			 "Alarm"}
	};


	private static AudioClip[] soundClips = new AudioClip[clipToName.Count]; 

	public static AudioClip GetSoundClip(SoundClip soundClip)
	{
		int index = (int)soundClip;
		if(soundClips[index] == null)
			soundClips[index] = Resources.Load("Audio/"+clipToName[soundClip], typeof(AudioClip)) as AudioClip;
		
		return soundClips[index];
	}
}
