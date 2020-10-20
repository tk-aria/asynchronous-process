using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
	/// <summary>
	///  [ここに何をするクラスなのかを記入する].
	/// </summary>
	public sealed class Sample : MonoBehaviour
	{
		#region Const
		#endregion // Const End.

		#region Type
		#endregion // Type End.

		#region Field

		AsyncProcess process = new AsyncProcess();

		#endregion // Field End.

		#region Property
		#endregion // Property End.

		#region Event
		#endregion // Event End.

		#region Method

		#region UnityCallback

		/// <summary>
		///  Unityによって呼び出される.
		/// </summary>
		private async void Start()
		{
            UnityEngine.Debug.Log("start");

            SleepCommand sleedCmd = new SleepCommand("5");
            {
                await process.StartCommandAsync(sleedCmd.Invoke());
            }

            UnityEngine.Debug.Log("end");
		}

		/// <summary>
		///  Unityから毎フレーム呼び出される.
		/// </summary>
		private void Update()
		{
		}

		#endregion // UnityCallback End.

		#endregion // Method End.
	}
}