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
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.Input.Touch
{
	// https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.touch.touchpanel.aspx
	public static class TouchPanel
	{
		#region Public Static Properties

		public static IntPtr WindowHandle
		{
			get;
			set;
		}

		public static int DisplayHeight
		{
			get;
			set;
		}

		public static DisplayOrientation DisplayOrientation
		{
			get;
			set;
		}

		public static int DisplayWidth
		{
			get;
			set;
		}

		public static GestureType EnabledGestures
		{
			get;
			set;
		}

		public static bool IsGestureAvailable
		{
			get
			{
				return GestureList.Count > 0;
			}
		}

		#endregion

		#region Internal Static Variables

		internal static bool TouchDeviceExists;

		#endregion

		#region Private Static Variables

		private static Queue<GestureSample> GestureList = new Queue<GestureSample>();
		private static List<TouchLocation> touchState = new List<TouchLocation>(MAX_TOUCHES);
		private static Queue<TouchLocation> touchEvents = new Queue<TouchLocation>();
		private static HashSet<int> touchIDsToRelease = new HashSet<int>();

		#endregion

		#region Private Constants

		// The maximum number of simultaneous touches allowed by XNA.
		private const int MAX_TOUCHES = 8;

		#endregion

		#region Public Static Methods

		public static TouchPanelCapabilities GetCapabilities()
		{
			return FNAPlatform.GetTouchCapabilities();
		}

		public static TouchCollection GetState()
		{
			if (!FNAPlatform.IsOnTouchPlatform())
			{
				return TouchCollection.Empty;
			}
			TouchCollection result = (touchState.Count > 0) ?
				new TouchCollection(touchState.ToArray()) :
				TouchCollection.Empty;
			return result;
		}

		public static GestureSample ReadGesture()
		{
			if (!FNAPlatform.IsOnTouchPlatform())
			{
				return default(GestureSample);
			}
			return GestureList.Dequeue();
		}

		#endregion

		#region Internal Static Methods

		internal static void EnqueueGesture(GestureSample gesture)
		{
			GestureList.Enqueue(gesture);
		}

		internal static void INTERNAL_onTouchEvent(
			int fingerId,
			TouchLocationState state,
			float x,
			float y,
			float dx,
			float dy
		) {
			// Calculate the scaled touch position
			Vector2 touchPos = new Vector2(
				(float) Math.Round(x * DisplayWidth),
				(float) Math.Round(y * DisplayHeight)
			);

			// Add the event to the queue
			touchEvents.Enqueue(new TouchLocation(fingerId, state, touchPos));

			// Notify the Gesture Detector about the event
			switch (state)
			{
				case TouchLocationState.Pressed:
					GestureDetector.OnPressed(fingerId, touchPos);
					break;

				case TouchLocationState.Moved:

					Vector2 delta = new Vector2(
						(float) Math.Round(dx * DisplayWidth),
						(float) Math.Round(dy * DisplayHeight)
					);

					GestureDetector.OnMoved(fingerId, touchPos, delta);

					break;

				case TouchLocationState.Released:
					GestureDetector.OnReleased(fingerId, touchPos);
					break;
			}
		}

		internal static void Update()
		{
			// Remove all touches that were released last frame
			touchState.RemoveAll(touch => touch.State == TouchLocationState.Released);

			// Update Gesture Detector for time-sensitive gestures
			GestureDetector.OnTick();

			// Save touch states and positions for future reference
			List<TouchLocation> prevTouches = new List<TouchLocation>(touchState);

			// Process Pressed touch events from last frame
			for (int i = 0; i < touchState.Count; i += 1)
			{
				if (touchState[i].State == TouchLocationState.Pressed)
				{
					// If this press was marked for release
					if (touchIDsToRelease.Contains(touchState[i].Id))
					{
						// Change the touch's state to Released
						touchState[i] = new TouchLocation(
							touchState[i].Id,
							TouchLocationState.Released,
							touchState[i].Position,
							prevTouches[i].State,
							prevTouches[i].Position
						);
					}
					else
					{
						// Change the touch's state to Moved
						touchState[i] = new TouchLocation(
							touchState[i].Id,
							TouchLocationState.Moved,
							touchState[i].Position,
							prevTouches[i].State,
							prevTouches[i].Position
						);
					}
				}
			}
			touchIDsToRelease.Clear();

			// Process new touch events
			while (touchEvents.Count > 0)
			{
				TouchLocation touchEvent = touchEvents.Dequeue();

				// Add a new touch to the list if we have room
				if (touchEvent.State == TouchLocationState.Pressed &&
					touchState.Count < MAX_TOUCHES)
				{
					touchState.Add(touchEvent);
				}
				else
				{
					// Update touches that were already registered
					for (int i = 0; i < touchState.Count; i += 1)
					{
						if (touchState[i].Id == touchEvent.Id)
						{
							if (touchState[i].State == TouchLocationState.Pressed)
							{
								// If the touch was pressed and released in the same frame
								if (touchEvent.State == TouchLocationState.Released)
								{
									// Mark it for release on the next frame
									touchIDsToRelease.Add(touchState[i].Id);
								}
							}
							else
							{
								// Update the existing touch with new data
								touchState[i] = new TouchLocation(
									touchState[i].Id,
									touchEvent.State,
									touchEvent.Position,
									prevTouches[i].State,
									prevTouches[i].Position
								);
							}

							// We found the touch we were looking for.
							break;
						}
					}
				}
			}
		}

		#endregion
	}
}
