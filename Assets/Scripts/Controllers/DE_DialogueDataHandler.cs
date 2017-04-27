using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_DialogueDataHandler : MonoBehaviour
{
	public string[] speakers;
	public Dialogue[] dialogues;
	public string[] triggers;

	// hidden
	public int dialogueIndex = 0;
	public int firstDialogueIndex = 0;

	public DE_DialogueController[] dControls;
	public DE_DialogueController firstDialogue 
	{
		get { return dControls [ firstDialogueIndex ]; }
	}

	//--------------------------------------------------
	// public
	//--------------------------------------------------
	public DE_DialogueController GetDialogueByID ( string _linkID )
	{
		if ( dControls != null )
		{
			foreach ( DE_DialogueController _dc in dControls )
			{
				if ( _dc.name.Equals ( _linkID ) )
					return _dc;
			}
		}
		return null;
	}

	public int GetDialogueIndexByID ( string _ID )
	{
		for ( int a = 0; a < dialogues.Length; ++a )
		{
			if ( dialogues [ a ].ID.Equals ( _ID ) )
				return a;
		}
		return -1;
	}

	public List<string> GetDialogueList ( string _exception = null )
	{
		List<string> _list = new List<string> ();
		_list.Add ( "none" );
		foreach ( string _t in triggers )
			_list.Add ( _t );
		foreach ( Dialogue _d in dialogues )
		{
			if ( !_exception.IsEmpty () && _d.ID.Equals ( _exception ) )
				continue;
			
			_list.Add ( _d.ID );
		}
		return _list;
	}

	//--------------------------------------------------
	// private
	//--------------------------------------------------
	private void GenerateDialogueGO ()
	{
		Transform _dparent = transform.FindChild ( "dialogues" );
		if ( _dparent != null )
			Destroy ( _dparent.gameObject );
		_dparent = new GameObject ( "dialogues" ).transform;
		_dparent.SetParent ( transform );

		// generate dialogue GOs w DialogueController, set their parent as this
		dControls = new DE_DialogueController[ dialogues.Length ];
		for ( int a=0; a<dialogues.Length; ++a )
		{
			GameObject _dgo = new GameObject ( dialogues[a].ID );
			_dgo.transform.SetParent ( _dparent );
			dControls[a] = _dgo.AddComponent<DE_DialogueController> ();
			dControls[a].dialogueData = dialogues[a].DeepClone ();
		}

		// foreach DialogueController, get all their linkage
		foreach ( DE_DialogueController _dc in dControls )
		{
			if ( _dc.dialogueData.links.Length > 0 )
			{
				_dc.links = new DE_DialogueController[ _dc.dialogueData.links.Length ];
				for ( int a = 0; a < _dc.links.Length; ++a )
					_dc.links [ a ] = GetDialogueByID ( _dc.dialogueData.links [ a ].linkID );
			}
		}
	}

	//--------------------------------------------------
	// protected mono
	//--------------------------------------------------
	protected void Awake ()
	{
		GenerateDialogueGO ();
	}

}