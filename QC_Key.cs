/* 
QuickContracts
Copyright 2015 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using UnityEngine;

namespace QuickContracts {
	public class QKey {
	
		internal static Key SetKey = Key.None;
		internal static Rect RectSetKey = new Rect();

		internal enum Key {
			None,
			DeclineSelectedContract,
			DeclineAllContracts,
			DeclineAllTest,
			AcceptSelectedContract
		}					
	
		internal static KeyCode DefaultKey(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return KeyCode.X;
			case Key.DeclineAllContracts:
				return KeyCode.C;
			case Key.DeclineAllTest:
				return KeyCode.V;
			case Key.AcceptSelectedContract:
				return KeyCode.A;
			}
			return KeyCode.None;
		}

		internal static string GetText(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return "Decline Selected Contract";
			case Key.DeclineAllContracts:
				return "Decline All Contracts";
			case Key.DeclineAllTest:
				return "Decline All Test";
			case Key.AcceptSelectedContract:
				return "Accept Selected Contract";
			}
			return string.Empty;
		}

		internal static KeyCode CurrentKey(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return QSettings.Instance.KeyDeclineSelectedContract;
			case Key.DeclineAllContracts:
				return QSettings.Instance.KeyDeclineAllContracts;
			case Key.DeclineAllTest:
				return QSettings.Instance.KeyDeclineAllTest;
			case Key.AcceptSelectedContract:
				return QSettings.Instance.KeyAcceptSelectedContract;
			}
			return KeyCode.None;
		}

		internal static void VerifyKey(Key key) {
			try {
				Input.GetKey(CurrentKey(key));
			} catch {
				Quick.Warning ("Wrong key: " + CurrentKey(key));
				SetCurrentKey (key, DefaultKey(key));
			}
		}

		internal static void VerifyKey() {
			VerifyKey (Key.DeclineSelectedContract);
			VerifyKey (Key.DeclineAllContracts);
			VerifyKey (Key.DeclineAllTest);
			VerifyKey (Key.AcceptSelectedContract);
		}

		internal static void SetCurrentKey(Key key, KeyCode CurrentKey) {
			switch (key) {
			case Key.DeclineSelectedContract:
				QSettings.Instance.KeyDeclineSelectedContract = CurrentKey;
				break;
			case Key.DeclineAllContracts:
				QSettings.Instance.KeyDeclineAllContracts = CurrentKey;
				break;
			case Key.DeclineAllTest:
				QSettings.Instance.KeyDeclineAllTest = CurrentKey;
				break;
			case Key.AcceptSelectedContract:
				QSettings.Instance.KeyAcceptSelectedContract = CurrentKey;
				break;
			}
		}

		internal static KeyCode GetKeyPressed() {
			string[] _keys = Enum.GetNames (typeof(KeyCode));
			int _length = _keys.Length;
			for (int _key = 0; _key < _length; _key++) {
				if (Input.GetKey((KeyCode)_key)) {
					return (KeyCode)_key;
				}
			}
			return KeyCode.None;
		}

		internal static void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format("Press a key to use a <color=#FFFFFF><b>{0}</b></color>",GetText(SetKey)));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Cancel Assignment", GUILayout.ExpandWidth(true), GUILayout.Height (30))) {
				SetKey = Key.None;
				QGUI.WindowSettings = true;
				return;
			}
			GUILayout.Space(5);
			if (GUILayout.Button ("Clear Assignment", GUILayout.ExpandWidth(true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
				QGUI.WindowSettings = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.EndVertical ();
		}

		internal static void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal();
			GUILayout.Label (string.Format("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText(key), CurrentKey(key)), GUILayout.Width (250));
			GUILayout.Space(5);
			if (GUILayout.Button ("Set", GUILayout.Width (25), GUILayout.Height (20))) {
				SetKey = key;
				QGUI.WindowSettings = false;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}
	}
}