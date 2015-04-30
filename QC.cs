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

using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuickContracts {
	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class QuickContracts : Quick {

		private void Awake() {
			if (HighLogic.LoadedSceneIsGame) {
				if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) {
					AutoDestroy ();
					return;
				}
			}
			QGUI.Awake ();
			QShortCuts.Awake ();
		}

		private void Start() {
			if (HighLogic.LoadedSceneIsGame) {
				if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
					QSettings.Instance.Load ();
					QShortCuts.VerifyKey ();
					GameEvents.onGUIMissionControlDespawn.Add (OnGUIMissionControlDespawn);
				}
			}
		}

		private void AutoDestroy() {
			Quick.Warning ("Can't work on Sandbox.");
			Destroy (this);
		}

		private void OnDestroy() {
			if (HighLogic.LoadedSceneIsGame) {
				if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) {
					GameEvents.onGUIMissionControlDespawn.Remove (OnGUIMissionControlDespawn);
				}
			}
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