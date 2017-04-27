//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DE_Helper
{
	public static bool IsEmpty ( this string _target )
	{
		return string.IsNullOrEmpty ( _target );
	}

	public static T DeepClone<T> ( this T _target )
	{
		string _stringify = JsonUtility.ToJson ( _target );
		return JsonUtility.FromJson<T> ( _stringify );
	}

	public static T[] RemoveAt<T> ( this T[] _target, int _index )
	{
		T[] _newValue = new T[ _target.Length - 1 ];
		if ( _index > 0 )
			System.Array.Copy ( _target, 0, _newValue, 0, _index );

		if ( _index < _target.Length - 1 )
			System.Array.Copy ( _target, _index + 1, _newValue, _index, _target.Length - _index - 1 );

		return _newValue;
	}

	public static T[] Remove<T> ( this T[] _target, T _item )
	{
		int _index = System.Array.IndexOf ( _target, _item );

		return _target.RemoveAt ( _index );
	}

	public static T[] Add<T>  ( this T[] _target, T _item )
	{
		T[] _added = new T[ _target.Length + 1 ];
		for ( int a = 0; a < _target.Length; ++a )
			_added [ a ] = _target [ a ];
		_added [ _target.Length ] = _item;
		return _added;
	}

	public static T[] Insert<T> ( this T[] _target, T _item, int _index )
	{
		T[] _newValue = new T[ _target.Length + 1 ];

		for ( int i=0; i<_index; i++ )
			_newValue[i] = _target[i];
		
		_newValue[ _index ] = _item;
		for ( int i= ( _index + 1 ); i<_newValue.Length; i++ )
			_newValue[i] = _target[ i - 1 ];
		
		return _newValue;
	}

	public static bool Contains<T>  ( this T[] _target, T _item )
	{
		foreach ( T _t in _target )
		{
			if ( _t.Equals ( _item ) )
				return true;
		}
		return false;
	}

	public static T[] SwapWithPrevious<T>  ( this T[] _target, int _currentIndex )
	{
		if ( _currentIndex > 0 )
		{
			int _prevIndex = _currentIndex - 1;
			T _previous = _target[_prevIndex];
			T _current = _target[_currentIndex];

			_target[_prevIndex] = _current;
			_target[_currentIndex] = _previous;
		}

		return _target;
	}

	public static T[] SwapWithNext<T>  ( this T[] _target, int _currentIndex )
	{
		if ( _currentIndex < _target.Length - 1 )
		{
			int _nextIndex = _currentIndex + 1;
			T _next = _target[_nextIndex];
			T _current = _target[_currentIndex];

			_target[_nextIndex] = _current;
			_target[_currentIndex] = _next;
		}

		return _target;
	}

	public static T[] InsertAbove<T>  ( this T[] _target, T _item, int _insertAt )
	{
		int _current = System.Array.IndexOf ( _target, _item );

		if ( _current >= 0 )
		{
			if ( _current != _insertAt - 1 )
			{
				if ( _current < _insertAt )
					--_insertAt;

				_target = _target.Remove ( _item );
				_target = _target.Insert ( _item, _insertAt );
			}
		}

		return _target;
	}

	public static T[] InsertBelow<T>  ( this T[] value, T _item, int _insertAt )
	{
		int _current = System.Array.IndexOf ( value, _item );

		if ( _current >= 0 )
		{
			if ( _current != _insertAt + 1 )
			{
				if ( _current > _insertAt )
					++_insertAt;

				value = value.Remove ( _item );
				value = value.Insert ( _item, _insertAt );
			}
		}

		return value;
	}

	public static int Min ( this int _target, int _min )
	{
		return Mathf.Min ( _target, _min );
	}

	public static int Max ( this int _target, int _max )
	{
		return Mathf.Max ( _target, _max );
	}

	public static Color MixColours ( Color[] _colours )
	{
		Color _mixed = Color.white;
		if ( _colours != null && _colours.Length > 0 )
		{
			float _dividePerc = 1f / ( float ) _colours.Length;
			_mixed = _colours[0] * _dividePerc;
			for ( int i=1; i<_colours.Length; i++ )
			{
				Color _colour = _colours[i];
				_mixed += _colour * _dividePerc;
			}
		}

		return _mixed;
	}

}
