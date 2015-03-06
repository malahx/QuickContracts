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
using System.IO;
using UnityEngine;

namespace QuickContracts {
	public class QSettings : Quick {

		public static QSettings Instance = new QSettings ();

		private string File_settings = KSPUtil.ApplicationRootPath + "GameData/" + MOD + "/Config.txt";

		[Persistent]
		internal KeyCode KeyDeclineSelectedContract;
		[Persistent]
		internal KeyCode KeyDeclineAllContracts;
		[Persistent]
		internal KeyCode KeyDeclineAllTest;
		[Persistent]
		internal KeyCode KeyAcceptSelectedContract;
		[Persistent]
		internal bool TestContracts = true;
		[Persistent]
		internal bool RescueContracts = true;
		[Persistent]
		internal bool ScienceDataContracts = true;
		[Persistent]
		internal bool SurveyContracts = true;
		[Persistent]
		internal bool StationContracts = true;
		[Persistent]
		internal bool BaseContracts = true;
		[Persistent]
		internal bool SatelliteContracts = true;
		[Persistent]
		internal bool ISRUContracts = true;
		[Persistent]
		internal bool ARMContracts = true;
		[Persistent]
		internal bool PlantFlagContracts = true;
		[Persistent]
		internal bool AltitudeRecordContracts = true;
		[Persistent]
		internal bool ExploreBodyContracts = true;
		[Persistent]
		internal bool FirstLaunchContracts = true;
		[Persistent]
		internal bool GrandTourContracts = true;
		[Persistent]
		internal bool OrbitKerbinContracts = true;
		[Persistent]
		internal bool ReachSpaceContracts = true;

		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(File_settings);
			Quick.Log ("Save");
		}
		public void Load() {
			if (File.Exists (File_settings)) {
				ConfigNode _temp = ConfigNode.Load (File_settings);
				ConfigNode.LoadObjectFromConfig (this, _temp);
				Quick.Log ("Load");
			} else {
				Save ();
			}
		}
	}
}