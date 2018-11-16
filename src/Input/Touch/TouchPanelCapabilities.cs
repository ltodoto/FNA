#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2018 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

namespace Microsoft.Xna.Framework.Input.Touch
{
	// https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.touch.touchpanelcapabilities.aspx
	public struct TouchPanelCapabilities
	{
		#region Public Properties

		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
		}

		public int MaximumTouchCount
		{
			get
			{
				return maximumTouchCount;
			}
		}

		#endregion

		#region Private Variables

		private bool isConnected;
		private int maximumTouchCount;
		private bool initialized;

		#endregion

		#region Internal Initialize Methods
		
		internal void Initialize()
		{
			if (!initialized)
			{
				initialized = true;
				isConnected = FNAPlatform.HasTouch();
				maximumTouchCount = FNAPlatform.GetMaximumTouchCount();
			}
		}
		
		internal void InitializeStub()
		{
			if (!initialized)
			{
				initialized = true;
				isConnected = false;
				maximumTouchCount = 0;
			}
		}

		#endregion
	}
}
