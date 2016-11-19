// ##############################################################################
//
// ICECreatureEBTAdapterEditor.cs
// Version 1.1.20
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using ICE;
using ICE.World;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.Utilities;

using EnergyBarToolkit;

namespace ICE.Creatures.Adapter
{


	[CustomEditor(typeof(ICECreatureEBTAdapter))]
	public class ICECreatureEBTAdapterEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			Info.HelpButtonIndex = 0;
			ICECreatureEBTAdapter _adapter = (ICECreatureEBTAdapter)target;

			EditorGUILayout.Separator();
			ICEEditorLayout.BeginHorizontal();
				_adapter.Display = (Canvas)EditorGUILayout.ObjectField("Display", _adapter.Display, typeof(Canvas), true);
			ICEEditorLayout.EndHorizontal();

			EditorGUILayout.Separator();
			ICEEditorLayout.Label( "Offsets", false );
			EditorGUI.indentLevel++;
				_adapter.InitialOffset = ICEEditorLayout.DefaultSlider( "Initial Offset", "", _adapter.InitialOffset, 0.01f, -10,10,0,"" );
				_adapter.LineOffset = ICEEditorLayout.DefaultSlider( "Line Offset", "", _adapter.LineOffset, 0.01f, 0,10,0,"" );
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			ICEEditorLayout.Label( "Status Bars", false );
			EditorGUI.indentLevel++;
			for( int _i = 0 ; _i < _adapter.Bars.Count ; _i++ )
			{
				ICEEnergyBarObject _bar = _adapter.Bars[_i];

				ICEEditorLayout.BeginHorizontal();
				_bar.Value = (ICEEnergyBarDataType)ICEEditorLayout.EnumPopup("Status Value #" + (_i+1), "", _bar.Value);

				if( ICEEditorLayout.ButtonUp() )
				{
					ICEEnergyBarObject _obj = _adapter.Bars[_i]; 
					_adapter.Bars.RemoveAt( _i );

					if( _i - 1 < 0 )
						_adapter.Bars.Add( _obj );
					else
						_adapter.Bars.Insert( _i - 1, _obj );

					return;
				}	


				if( ICEEditorLayout.ButtonDown() )
				{
					ICEEnergyBarObject _obj = _adapter.Bars[_i]; 
					_adapter.Bars.RemoveAt( _i );

					if( _i + 1 > _adapter.Bars.Count )
						_adapter.Bars.Insert( 0, _obj );
					else
						_adapter.Bars.Insert( _i +1, _obj );

					return;
				}	

				if (GUILayout.Button("COPY", ICEEditorStyle.CMDButtonDouble ))
					_adapter.Bars.Add ( new ICEEnergyBarObject( _bar ) );

				if (GUILayout.Button("X", ICEEditorStyle.CMDButton ))
				{
					_adapter.Bars.RemoveAt(_i);
					--_i;
				}
				ICEEditorLayout.EndHorizontal();

				EditorGUI.indentLevel++;
				ICEEditorLayout.BeginHorizontal();
					_bar.ReferenceObject = (EnergyBar)EditorGUILayout.ObjectField("Reference", _bar.ReferenceObject, typeof(EnergyBar), true);
				ICEEditorLayout.EndHorizontal();

				ICEEditorLayout.BeginHorizontal();
				_bar.Offset = ICEEditorLayout.OffsetGroup("Offset", _bar.Offset);
				ICEEditorLayout.EndHorizontal();

				_bar.VerticalOffset = _adapter.InitialOffset + ( _adapter.LineOffset * ( _adapter.Bars.Count - _i) );


				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;

			ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.Label( "Add Status Value", false );
				GUILayout.FlexibleSpace();
				if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
					_adapter.Bars.Add( new ICEEnergyBarObject() );
			ICEEditorLayout.EndHorizontal();

			EditorGUILayout.Separator();
		}
	}
}
