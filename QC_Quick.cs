using System;
using System.Reflection;
using UnityEngine;

namespace QuickContracts {
	public class Quick : MonoBehaviour {
		public readonly static string VERSION = Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickContracts)).GetName().Version.Build;
		public readonly static string MOD = Assembly.GetAssembly(typeof(QuickContracts)).GetName().Name;
		private static bool isdebug = true;

		// Afficher les messages sur la console
		internal static void Log(string _string) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + _string);
			}
		}
		internal static void Warning(string _string) {
			if (isdebug) {
				Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
			}
		}
	}
}

