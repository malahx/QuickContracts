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
using System.Text.RegularExpressions;
using UnityEngine;

namespace QuickContracts {

	public class QContracts : Quick {

		internal static void DisableContract(Type ContractType) {
			ContractSystem.ContractTypes.Remove (ContractType);
			Log ("Disable: " + ContractType.Name);
			DeclineAll (ContractType);
		}

		internal static void DeclineAll(Type ContractType) {
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			foreach (Contract _contract in _contracts) {
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined () && _contract.GetType() == ContractType) {
					_contract.Decline ();
				}
			}
			Clear ();
			Log ("Decline all: " + ContractType.Name);

		}

		internal static void DeclineAll() {
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			foreach (Contract _contract in _contracts) {
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined ()) {
					_contract.Decline ();
				}
			}
			Clear ();
			Log ("Decline all contracts");
		}

		internal static void Clear() {
			if (MissionControl.Instance != null) {
				MissionControl.Instance.UpdateInfoPanelContract (null);
				MissionControl.Instance.UpdateInfoPanelAgent (null);
				MissionControl.Instance.missionPanelManager.Dismiss (UIPanelManager.MENU_DIRECTION.Forwards);
				if (MissionControl.Instance.scrollListAvailable.LastClickedControl != null) {
					MissionControl.Instance.scrollListAvailable.DidClick (MissionControl.Instance.scrollListAvailable.LastClickedControl);
				}
				if (MissionControl.Instance.scrollListActive.LastClickedControl != null) {
					MissionControl.Instance.scrollListActive.DidClick (MissionControl.Instance.scrollListActive.LastClickedControl);
				}
				if (MissionControl.Instance.scrollListCancelled.LastClickedControl != null) {
					MissionControl.Instance.scrollListCancelled.DidClick (MissionControl.Instance.scrollListCancelled.LastClickedControl);
				}
				if (MissionControl.Instance.scrollListCompleted.LastClickedControl != null) {
					MissionControl.Instance.scrollListCompleted.DidClick (MissionControl.Instance.scrollListCompleted.LastClickedControl);
				}
				if (MissionControl.Instance.scrollListFailed.LastClickedControl != null) {
					MissionControl.Instance.scrollListFailed.DidClick (MissionControl.Instance.scrollListFailed.LastClickedControl);
				}
				if (MissionControl.Instance.scrollListFinished.LastClickedControl != null) {
					MissionControl.Instance.scrollListFinished.DidClick (MissionControl.Instance.scrollListFinished.LastClickedControl);
				}
			}
		}
	}
}