#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2018 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
#endregion

namespace Microsoft.Xna.Framework.Input.Touch
{
	public struct GestureSample
	{

		#region Public Properties

		public GestureType GestureType
		{
			get
			{
				return gestureType;
			}
		}

		public TimeSpan Timestamp
		{
			get
			{
				return timestamp;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}

		public Vector2 Position2
		{
			get
			{
				return position2;
			}
		}

		public Vector2 Delta
		{
			get
			{
				return delta;
			}
		}

		public Vector2 Delta2
		{
			get
			{
				return delta2;
			}
		}

		#endregion

		#region Private Variables

		private GestureType gestureType;
		private TimeSpan timestamp;
		private Vector2 position;
		private Vector2 position2;
		private Vector2 delta;
		private Vector2 delta2;

		#endregion

		#region Public Constructor

		public GestureSample(
			GestureType gestureType,
			TimeSpan timestamp,
			Vector2 position,
			Vector2 position2,
			Vector2 delta,
			Vector2 delta2
		) {
			this.gestureType = gestureType;
			this.timestamp = timestamp;
			this.position = position;
			this.position2 = position2;
			this.delta = delta;
			this.delta2 = delta2;
		}

		#endregion
	}
}
