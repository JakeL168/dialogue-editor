using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_DialogueSample : MonoBehaviour
{
	public DE_DialogueDataHandler[] sampleData;

	private bool tryGetDialogueUI = false;
	private DEUI_Dialogue _dialogueUI;
	public DEUI_Dialogue dialogueUI
	{
		get
		{
			if ( !tryGetDialogueUI )
			{
				_dialogueUI = GetComponentInChildren<DEUI_Dialogue> ();
				tryGetDialogueUI = true;
			}
			return _dialogueUI;
		}
	}

	//--------------------------------------------------
	// public
	//--------------------------------------------------
	public void OnClickSample ( int _index )
	{
		dialogueUI.activeData = sampleData [ _index ];
	}

	//--------------------------------------------------
	// private
	//--------------------------------------------------

	//--------------------------------------------------
	// protected mono
	//--------------------------------------------------


}
