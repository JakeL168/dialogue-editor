using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ CustomEditor ( typeof ( DE_DialogueDataHandler ) ) ]
public class DE_DialogueDataHandlerEditor : Editor 
{
	private Vector2 dialogueScroll = Vector2.zero;
	private bool showDeleteConfirmation = true;
	private int jumpState = -1;

	private void DrawLine ()
	{
		GUILayout.Box ( "", GUILayout.Width ( EditorGUIUtility.currentViewWidth - 25f ), GUILayout.Height ( 1f ) );
	}

	public override void OnInspectorGUI ()
	{
		DE_DialogueDataHandler _target = ( DE_DialogueDataHandler )target;

		if ( _target.speakers == null
			|| _target.speakers.Length <= 0 )
			_target.speakers = new string[1]{ "DAN" };

		if ( _target.triggers == null )
			_target.triggers = new string[0];

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.BeginVertical ( GUILayout.Width ( 150f ) );
			{
				EditorGUILayout.LabelField ( "SPEAKER" );
				for ( int a = 0; a < _target.speakers.Length; ++a )
				{
					EditorGUILayout.BeginHorizontal ( GUILayout.Width ( 150f ) );
					{
						EditorGUILayout.LabelField ( a + ".", GUILayout.Width ( 20f ) );
						_target.speakers [ a ] = GUILayout.TextField ( _target.speakers [ a ] );

						EditorGUI.BeginDisabledGroup ( _target.speakers.Length <= 1 );
						{
							GUI.color = Color.red;
							if ( GUILayout.Button ( "-", GUILayout.Width ( 20f ) ) )
							{
								_target.speakers = _target.speakers.RemoveAt ( a );
								return;
							}
							GUI.color = Color.white;
						}
						EditorGUI.EndDisabledGroup ();
					}
					EditorGUILayout.EndHorizontal ();
				}

				GUI.color = Color.green;
				if ( GUILayout.Button ( "Add New Speaker", GUILayout.Width ( 150f ) ) )
				{
					_target.speakers = _target.speakers.Add ( "speaker_no" + _target.speakers.Length );
					return;
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndVertical ();

			EditorGUILayout.Space ();

			EditorGUILayout.BeginVertical ();
			{
				EditorGUILayout.LabelField ( "TRIGGER" );
				for ( int a = 0; a < _target.triggers.Length; ++a )
				{
					EditorGUILayout.BeginHorizontal ( GUILayout.Width ( 150f ) );
					{
						EditorGUILayout.LabelField ( a + ".", GUILayout.Width ( 20f ) );
						_target.triggers [ a ] = GUILayout.TextField ( _target.triggers [ a ] );

						GUI.color = Color.red;
						if ( GUILayout.Button ( "-", GUILayout.Width ( 20f ) ) )
						{
							foreach ( Dialogue _dialogue in _target.dialogues )
							{
								for ( int b=0; b<_dialogue.links.Length; ++b )
								{
									if ( _dialogue.links [ b ].linkID.Equals ( _target.triggers [ a ] ) )
										_dialogue.links [ b ].linkID = "none";
								}		
							}
							_target.triggers = _target.triggers.RemoveAt ( a );
							return;
						}
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal ();
				}

				GUI.color = Color.green;
				if ( GUILayout.Button ( "Add New Trigger", GUILayout.Width ( 150f ) ) )
				{
					_target.triggers = _target.triggers.Add ( "Trigger" + _target.triggers.Length );
					return;
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndHorizontal ();

		if ( _target.dialogues == null )
			_target.dialogues = new Dialogue[0];

		_target.dialogueIndex = Mathf.Clamp ( _target.dialogueIndex, 0, _target.dialogues.Length - 1 ).Max ( 0 );
		Dialogue _d = ( _target.dialogues.Length > 0 ) ? _target.dialogues [ _target.dialogueIndex ] : null;
		_target.firstDialogueIndex = Mathf.Clamp ( _target.firstDialogueIndex, 0, _target.dialogues.Length );
		List<string> _dLinks = new List<string> ();
		if ( _d != null )
		{
			if ( _d.links == null )
				_d.links = new Dialogue.Linkage[0];

			foreach ( Dialogue.Linkage _l in _d.links )
			{
				if ( _l.linkID.IsEmpty ()
					|| _l.linkID.Equals ( "none" ) )
					continue;

				_dLinks.Add ( _l.linkID );
			}
		}
		
		DrawLine ();

		EditorGUILayout.LabelField ( "DIALOGUE" );
		EditorGUILayout.BeginHorizontal ();
		{
			if ( _target.dialogues.Length > 0 )
			{
				EditorGUILayout.BeginVertical ( GUILayout.Width ( 180f ) );

				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUI.BeginDisabledGroup ( _target.dialogueIndex <= 0 );
					if ( GUILayout.Button ( "<" ) )
					{
						--_target.dialogueIndex;
						return;
					}
					EditorGUI.EndDisabledGroup ();

					EditorGUI.BeginDisabledGroup ( _target.dialogueIndex >= _target.dialogues.Length - 1 );
					if ( GUILayout.Button ( ">" ) )
					{
						++_target.dialogueIndex;
						return;
					}
					EditorGUI.EndDisabledGroup ();
				}
				EditorGUILayout.EndHorizontal ();

				dialogueScroll = EditorGUILayout.BeginScrollView ( dialogueScroll, false, false );
				for ( int a = 0; a < _target.dialogues.Length; ++a )
				{
					EditorGUILayout.BeginHorizontal ();
					GUI.color = DE_Helper.MixColours ( new Color[3]{ Color.blue, Color.red, Color.white } );
					if ( _target.dialogues [ a ].speaker.IsEmpty () )
						GUILayout.Box ( "N/A", GUILayout.Width ( 20f ) );
					else
						GUILayout.Box ( _target.dialogues [ a ].speaker[0].ToString (), GUILayout.Width ( 20f ) );

					if ( a == _target.firstDialogueIndex )
					{
						GUI.color = Color.yellow;
						GUILayout.Box ( "1st", GUILayout.Width ( 30f ) );
						GUI.color = Color.white;
					}

					if ( a == _target.dialogueIndex )
						GUI.color = DE_Helper.MixColours ( new Color[2]{ Color.blue, Color.white } );
					else if ( _dLinks.Contains ( _target.dialogues [ a ].ID ) )
						GUI.color = DE_Helper.MixColours ( new Color[2]{ Color.green, Color.white } );
					else
						GUI.color = Color.white;
				
					if ( GUILayout.Button ( _target.dialogues [ a ].ID ) )
					{
						if ( jumpState >= 0 )
						{
							if ( jumpState == 0 ) // jump above
							{
								_target.dialogues = _target.dialogues.InsertAbove ( _d, a );
								_target.dialogueIndex = ( a - 1 );
							}
							else // jump below
							{
								_target.dialogues = _target.dialogues.InsertBelow ( _d, a );
								_target.dialogueIndex = ( a + 1 );
							}
							jumpState = -1;
							return;
						}
						else
						{
							_target.dialogueIndex = a;
							GUI.FocusControl ( "" );
							return;
						}
					}
					EditorGUILayout.EndHorizontal ();
				}
				GUI.color = Color.white;
				EditorGUILayout.EndScrollView ();

				EditorGUILayout.EndVertical ();
			}

			EditorGUILayout.BeginVertical ();
			{
				// DIALOGUE editing
				if ( _d != null )
				{
					// CONTROLS
					EditorGUILayout.BeginHorizontal ();
					{
						GUI.color = Color.green;
						EditorGUI.BeginDisabledGroup ( _target.dialogueIndex <= 0 );
						{
							if ( GUILayout.Button ( "Move Up", GUILayout.Width ( 80f ) ) )
							{
								_target.dialogues = _target.dialogues.SwapWithPrevious ( _target.dialogueIndex );
								--_target.dialogueIndex;
								return;
							}
						}
						EditorGUI.EndDisabledGroup ();

						EditorGUI.BeginDisabledGroup ( _target.dialogueIndex >= _target.dialogues.Length - 1 );
						if ( GUILayout.Button ( "Move Down", GUILayout.Width ( 80f ) ) )
						{
							_target.dialogues = _target.dialogues.SwapWithNext ( _target.dialogueIndex );
							++_target.dialogueIndex;
							return;
						}
						EditorGUI.EndDisabledGroup ();

						GUILayout.Space ( 10f );
						if ( jumpState == 0 )
							GUI.color = DE_Helper.MixColours ( new Color[2]{ Color.green, Color.black } );
						else
							GUI.color = Color.green;
						if ( GUILayout.Button ( "Jump Above", GUILayout.Width ( 80f ) ) )
							jumpState = ( jumpState != 0 ) ? 0 : -1;

						if ( jumpState == 1 )
							GUI.color = DE_Helper.MixColours ( new Color[2]{ Color.green, Color.black } );
						else
							GUI.color = Color.green;
						if ( GUILayout.Button ( "Jump Below", GUILayout.Width ( 80f ) ) )
							jumpState = ( jumpState != 1 ) ? 1 : -1;
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.Space ();

					// ID
					EditorGUILayout.BeginHorizontal ();
					{
						EditorGUILayout.LabelField ( "ID", GUILayout.Width ( 80f ) );
						_d.ID = EditorGUILayout.TextField ( _d.ID );
					}
					EditorGUILayout.EndHorizontal ();

					// SPEAKER
					EditorGUILayout.BeginHorizontal ();
					{
						EditorGUILayout.LabelField ( "Speaker", GUILayout.Width ( 80f ) );
						if ( _d.speaker.IsEmpty () 
							|| !_target.speakers.Contains ( _d.speaker ) )
							_d.speaker = _target.speakers [ 0 ];
						int _sIndex = System.Array.IndexOf ( _target.speakers, _d.speaker );
						_d.speaker = _target.speakers[ EditorGUILayout.Popup ( _sIndex, _target.speakers ) ];
					}
					EditorGUILayout.EndHorizontal ();

					// CONTENT
					EditorGUILayout.LabelField ( "Content" );

					EditorGUILayout.BeginHorizontal ();
					GUILayout.Space ( 50f );
					GUIStyle _style = new GUIStyle ();
					_style.wordWrap = true;
					_style.normal.textColor = Color.blue;
					_style.alignment = TextAnchor.MiddleLeft;
					_d.content = EditorGUILayout.TextArea ( _d.content, _style );
					if ( _d.content.IsEmpty () )
						_d.content = "<insert text here>";
					GUILayout.Space ( 50f );
					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.Space ();

					if ( _d.links == null )
						_d.links = new Dialogue.Linkage[0];

					List<string> _list = _target.GetDialogueList ( _d.ID );
					for ( int a = 0; a < _d.links.Length; ++a )
					{
						GUI.color = DE_Helper.MixColours ( new Color[2]{ Color.green, Color.white } );
						EditorGUILayout.BeginHorizontal ();
						{
							EditorGUILayout.LabelField ( "Link " + a, GUILayout.Width ( 80f ) );
							_d.links [ a ].content = EditorGUILayout.TextField ( _d.links [ a ].content );
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						{
							if ( _d.links [ a ].linkID.IsEmpty ()
								|| !_list.Contains ( _d.links[a].linkID ) )
								_d.links [ a ].linkID = "none";
							
							int _index = _list.IndexOf ( _d.links [ a ].linkID );
							EditorGUILayout.LabelField ( "Link To", GUILayout.Width ( 80f ) );
							_d.links [ a ].linkID = _list [ EditorGUILayout.Popup ( _index, _list.ToArray () ) ];
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						{
							EditorGUILayout.Space ();

							GUI.color = Color.green;
							if ( GUILayout.Button ( "Go to Link", GUILayout.Width ( 100f ) ) )
							{
								_target.dialogueIndex = _target.GetDialogueIndexByID ( _d.links [ a ].linkID );
								return;
							}

							GUI.color = Color.red;
							if ( GUILayout.Button ( "Delete Linkage", GUILayout.Width ( 100f ) ) )
							{
								if ( !showDeleteConfirmation 
									|| EditorUtility.DisplayDialog ( "Delete this linkage?", "You can't UNDO this.", "Yes", "No" ) )
								{
									_d.links = _d.links.RemoveAt ( a );
									break;
								}
							}
							GUI.color = Color.white;
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
					}

					EditorGUILayout.Space ();
					GUI.color = Color.green;
					if ( GUILayout.Button ( "Add New Link" ) )
						_d.links = _d.links.Add ( new Dialogue.Linkage ( "..." ) );
					GUI.color = Color.white;

					// LAST : reassign back to _d for just incase
					_target.dialogues [ _target.dialogueIndex ] = _d;

					EditorGUILayout.BeginHorizontal ();
					{
						// MAKE FIRST
						EditorGUI.BeginDisabledGroup ( _target.dialogueIndex == _target.firstDialogueIndex );
						{
							if ( GUILayout.Button ( "Set As First" ) )
							{
								_target.firstDialogueIndex = _target.dialogueIndex;
								return;
							}
						}
						EditorGUI.EndDisabledGroup ();

						// DELETE
						GUI.color = Color.red;
						if ( GUILayout.Button ( "Delete Current" ) )
						{
							if ( !showDeleteConfirmation
							     || EditorUtility.DisplayDialog ( "Delete this dialogue?", "You can't UNDO this.", "Yes", "No" ) )
							{
								_target.dialogues = _target.dialogues.RemoveAt ( _target.dialogueIndex );
								return;
							}
						}
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal ();
		{
			GUI.color = Color.green;
			if ( GUILayout.Button ( "Add New Dialogue", GUILayout.Width ( 160f ) ) )
			{
				_target.dialogues = _target.dialogues.Add ( new Dialogue ( "New Dialogue " + _target.dialogues.Length ) );
				if ( _target.dialogues.Length > 1 )
					_target.dialogues [ _target.dialogues.Length - 1 ].speaker = _target.dialogues [ _target.dialogues.Length - 2 ].speaker;
				_target.dialogueIndex = _target.dialogues.Length - 1;
				return;
			}
			GUI.color = Color.white;

			showDeleteConfirmation = EditorGUILayout.ToggleLeft ( "  Show delete confirmation", showDeleteConfirmation );
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Space ();
	}
}
