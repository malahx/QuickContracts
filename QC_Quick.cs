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
using System.Reflection;
using UnityEngine;

namespace QuickContracts {
	public partial class QuickContracts : MonoBehaviour {
		public readonly static string VERSION = Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Build;
		public readonly static string MOD = Assembly.GetAssembly(typeof(QuickContracts)).GetName().Name;

		internal static void Log(string _string) {
			Debug.Log (MOD + "(" + VERSION + "): " + _string);
		}
		internal static void Warning(string _string) {
			Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
		}
	}
}

