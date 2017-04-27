using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ System.Serializable ]
public class Dialogue
{
	public Dialogue () {}
	public Dialogue ( string _ID ) 
	{
		ID = _ID;
	}

	public string ID;
	public string speaker;
	public string content;
	public Vector2 UIPos;

	[ System.Serializable ]
	public class Linkage
	{
		public Linkage () {}
		public Linkage ( string _content )
		{
			content = _content;
		}

		public string content;
		public string linkID;
	}
	public Linkage[] links;
}

public class DE_DialogueController : MonoBehaviour
{
	public System.Action<string> OnDialogueTrigger;

	public Dialogue dialogueData;
	public DE_DialogueController[] links;

	public DE_DialogueController Next ( int _index = 0 )
	{
		if ( dialogueData.links == null
			|| dialogueData.links.Length <= _index )
			return null;

		// check if it's trigger
		string _link = dialogueData.links [ _index ].linkID;
		string _triggerText = "[trigger]";
		if ( _link.Length > _triggerText.Length
			&& _link.Substring ( 0, _triggerText.Length ).Equals ( _triggerText ) )
		{
			// trigger something
			if ( OnDialogueTrigger != null )
				OnDialogueTrigger ( _link.Substring ( _triggerText.Length ) );
			return null;
		}
		else
			return links [ _index ];
	}

}
