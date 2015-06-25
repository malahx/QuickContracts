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
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public partial class QuickContracts : Quick {

		public static QuickContracts Instance {
			get;
			private set;
		}

		private void Awake() {
			if (Instance != null || HighLogic.CurrentGame.Mode != Game.Modes.CAREER) {
				Warning ("Destroy");
				Destroy (this);
				return;
			}
			Instance = this;
			QShortCuts.Awake ();
		}

		private void Start() {
			QSettings.Instance.Load ();
			QShortCuts.VerifyKey ();
			GameEvents.onGUIMissionControlDespawn.Add (OnGUIMissionControlDespawn);
		}

		private void OnDestroy() {
			GameEvents.onGUIMissionControlDespawn.Remove (OnGUIMissionControlDespawn);
		}

		private void OnGUIMissionControlDespawn() {
			QSettings.Instance.Save ();
		}

		private void Update() {
			QShortCuts.Update ();
		}

		private void OnGUI() {
			GUI.skin = HighLogic.Skin;
			QShortCuts.OnGUI ();
			QGUI.OnGUI ();
		}
	}
}