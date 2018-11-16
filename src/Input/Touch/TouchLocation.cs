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
using System.Diagnostics;
#endregion

namespace Microsoft.Xna.Framework.Input.Touch
{
	public struct TouchLocation : IEquatable<TouchLocation>
	{
		#region Public Properties

		public int Id
		{
			get
			{
				return id;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}
		
		public float PressureEXT
		{
			get
			{
				return pressure;
			}
		}

		public TouchLocationState State
		{
			get
			{
				return state;
			}
		}

		#endregion

		#region Internal Properties

		internal Vector2 PressPosition
		{
			get
			{
				return pressPosition;
			}
		}

		internal TimeSpan PressTimestamp
		{
			get
			{
				return pressTimestamp;
			}
		}

		internal TimeSpan Timestamp
		{
			get
			{
				return timestamp;
			}
		}

		internal Vector2 Velocity
		{
			get
			{
				return velocity;
			}
		}

		#endregion

		#region Internal Static TouchLocation

		internal static readonly TouchLocation Invalid = new TouchLocation();

		#endregion

		#region Internal Variables

		internal bool SameFrameReleased;

		#endregion

		#region Private Variables

		private int id;
		private Vector2 position;
		private Vector2 previousPosition;
		private float pressure;
		private float previousPressure;
		private TouchLocationState state;
		private TouchLocationState previousState;

		// Used for gesture recognition.
		private Vector2 velocity;
		private Vector2 pressPosition;
		private TimeSpan pressTimestamp;
		private TimeSpan timestamp;

		#endregion

		#region Public Constructors

		public TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position
		) : this(
			id,
			state,
			position,
			TouchLocationState.Invalid,
			Vector2.Zero,
			TimeSpan.Zero
		) {
		}
		
		public TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			TouchLocationState previousState,
			Vector2 previousPosition
		) : this(
			id,
			state,
			position,
			previousState,
			previousPosition,
			TimeSpan.Zero
		) {
		}
		
		public TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			float pressure
		) : this(
			id,
			state,
			position,
			pressure,
			TouchLocationState.Invalid,
			Vector2.Zero,
			0f,
			TimeSpan.Zero
		) {
		}

		public TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			float pressure,
			TouchLocationState previousState,
			Vector2 previousPosition,
			float previousPressure
		) : this(
			id,
			state,
			position,
			pressure,
			previousState,
			previousPosition,
			previousPressure,
			TimeSpan.Zero
		) {
		}

		#endregion

		#region Internal Constructors

		internal TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			TimeSpan timestamp
		) : this(
			id,
			state,
			position,
			TouchLocationState.Invalid,
			Vector2.Zero,
			timestamp
		) {
		}
		
		internal TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			float pressure,
			TimeSpan timestamp
		) : this(
			id,
			state,
			position,
			pressure,
			TouchLocationState.Invalid,
			Vector2.Zero,
			0f,
			timestamp
		) {
		}
		
		internal TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			TouchLocationState previousState,
			Vector2 previousPosition,
			TimeSpan timestamp
		) : this(
			id,
			state,
			position,
			0f,
			previousState,
			previousPosition,
			0f,
			timestamp
		) {
		}

		internal TouchLocation(
			int id,
			TouchLocationState state,
			Vector2 position,
			float pressure,
			TouchLocationState previousState,
			Vector2 previousPosition,
			float previousPressure,
			TimeSpan timestamp
		) {
			this.id = id;
			this.state = state;
			this.position = position;
			this.pressure = pressure;

			this.previousState = previousState;
			this.previousPosition = previousPosition;
			this.previousPressure = previousPressure;

			this.timestamp = timestamp;
			velocity = Vector2.Zero;

			/* If this is a pressed location then store the current position
			 * and timestamp as pressed.
			 */
			if (state == TouchLocationState.Pressed)
			{
				pressPosition = this.position;
				pressTimestamp = this.timestamp;
			}
			else
			{
				pressPosition = Vector2.Zero;
				pressTimestamp = TimeSpan.Zero;
			}

			SameFrameReleased = false;
		}

		#endregion

		#region Public Methods

		public bool TryGetPreviousLocation(out TouchLocation aPreviousLocation)
		{
			if (previousState == TouchLocationState.Invalid)
			{
				aPreviousLocation.id = -1;
				aPreviousLocation.state = TouchLocationState.Invalid;
				aPreviousLocation.position = Vector2.Zero;
				aPreviousLocation.pressure = 0f;
				aPreviousLocation.previousState = TouchLocationState.Invalid;
				aPreviousLocation.previousPosition = Vector2.Zero;
				aPreviousLocation.previousPressure = 0f;
				aPreviousLocation.timestamp = TimeSpan.Zero;
				aPreviousLocation.pressPosition = Vector2.Zero;
				aPreviousLocation.pressTimestamp = TimeSpan.Zero;
				aPreviousLocation.velocity = Vector2.Zero;
				aPreviousLocation.SameFrameReleased = false;
				return false;
			}

			aPreviousLocation.id = id;
			aPreviousLocation.state = previousState;
			aPreviousLocation.position = previousPosition;
			aPreviousLocation.pressure = previousPressure;
			aPreviousLocation.previousState = TouchLocationState.Invalid;
			aPreviousLocation.previousPosition = Vector2.Zero;
			aPreviousLocation.previousPressure = 0f;
			aPreviousLocation.timestamp = timestamp;
			aPreviousLocation.pressPosition = pressPosition;
			aPreviousLocation.pressTimestamp = pressTimestamp;
			aPreviousLocation.velocity = velocity;
			aPreviousLocation.SameFrameReleased = SameFrameReleased;
			return true;
		}

		#endregion

		#region Public IEquatable Methods and Operator Overrides

		public override int GetHashCode()
		{
			return id;
		}

		public override string ToString()
		{
			return "{Position:" + position.ToString() + "}";
		}

		public override bool Equals(object obj)
		{
			if (obj is TouchLocation)
			{
				return Equals((TouchLocation)obj);
			}

			return false;
		}

		public bool Equals(TouchLocation other)
		{
			return (	id.Equals(other.id) &&
					position.Equals(other.position) &&
					previousPosition.Equals(other.previousPosition) &&
					pressure.Equals(other.pressure) &&
					previousPressure.Equals(other.previousPressure)	);
		}

		public static bool operator !=(TouchLocation value1, TouchLocation value2)
		{
			return (	value1.id != value2.id ||
					value1.state != value2.state ||
					value1.position != value2.position ||
					value1.pressure != value2.pressure ||
					value1.previousState != value2.previousState ||
					value1.previousPosition != value2.previousPosition ||
					value1.previousPressure != value2.previousPressure	);
		}

		public static bool operator ==(TouchLocation value1, TouchLocation value2)
		{
			return (	value1.id == value2.id &&
					value1.state == value2.state &&
					value1.position == value2.position &&
					value1.pressure == value2.pressure &&
					value1.previousState == value2.previousState &&
					value1.previousPosition == value2.previousPosition &&
					value1.previousPressure == value2.previousPressure	);
		}

		#endregion

		#region Internal Methods

		internal TouchLocation AsMovedState()
		{
			TouchLocation touch = this;

			// Store the current state as the previous.
			touch.previousState = touch.state;
			touch.previousPosition = touch.position;
			touch.previousPressure = touch.pressure;

			// Set the new state.
			touch.state = TouchLocationState.Moved;

			return touch;
		}

		internal bool UpdateState(TouchLocation touchEvent)
		{
			Debug.Assert(
				Id == touchEvent.Id,
				"The touch event must have the same Id!"
			);
			Debug.Assert(
				State != TouchLocationState.Released,
				"We shouldn't be changing state on a released location!"
			);
			Debug.Assert(
				touchEvent.State == TouchLocationState.Moved || touchEvent.State == TouchLocationState.Released,
				"The new touch event should be a move or a release!"
			);
			Debug.Assert(
				touchEvent.Timestamp >= timestamp,
				"The touch event is older than our timestamp!"
			);

			// Store the current state as the previous one.
			previousPosition = position;
			previousPressure = pressure;
			previousState = state;

			// Set the new state.
			position = touchEvent.position;
			pressure = touchEvent.pressure;
			if (touchEvent.State == TouchLocationState.Released)
			{
				state = touchEvent.state;
			}

			// If time has elapsed then update the velocity.
			Vector2 delta = position - previousPosition;
			TimeSpan elapsed = touchEvent.Timestamp - timestamp;
			if (elapsed > TimeSpan.Zero)
			{
				// Use a simple low pass filter to accumulate velocity.
				Vector2 vel = delta / (float) elapsed.TotalSeconds;
				velocity += (vel - velocity) * 0.45f;
			}

			// Going straight from pressed to released on the same frame
			if (	previousState == TouchLocationState.Pressed &&
				state == TouchLocationState.Released &&
				elapsed == TimeSpan.Zero	)
			{
				// Lie that we are pressed for now
				SameFrameReleased = true;
				state = TouchLocationState.Pressed;
			}

			// Set the new timestamp.
			timestamp = touchEvent.Timestamp;

			// Return true if the state actually changed.
			return state != previousState || delta.LengthSquared() > 0.001f;
		}

		internal void AgeState()
		{
			Debug.Assert(
				state == TouchLocationState.Pressed,
				"Can only age the state of touches that are in the Pressed State"
			);

			previousState = state;
			previousPosition = position;
			previousPressure = pressure;

			if (SameFrameReleased)
			{
				state = TouchLocationState.Released;
			}
			else
			{
				state = TouchLocationState.Moved;
			}
		}

		#endregion
	}
}
