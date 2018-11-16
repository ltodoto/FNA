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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Microsoft.Xna.Framework.Input.Touch
{
	public struct TouchCollection : IList<TouchLocation>
	{
		#region Public Properties

		public int Count
		{
			get
			{
				return Collection.Length;
			}
		}

		public bool IsConnected 
		{
			get
			{
				return TouchPanel.TouchDeviceExists;
			}
		}

		#endregion

		#region Public IList<TouchLocation> Properties

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public TouchLocation this[int index]
		{
			get
			{
				return Collection[index];
			}
			set
			{
				// This will cause a runtime exception
				Collection[index] = value;
			}
		}

		#endregion

		#region Private Properties

		private TouchLocation[] Collection
		{
			get
			{
				return collection ?? EmptyLocationArray;
			}
		}

		#endregion

		#region Internal Static Variables

		internal static readonly TouchCollection Empty = new TouchCollection(
			new TouchLocation[] {}
		);

		#endregion

		#region Private Variables

		private TouchLocation[] collection;

		#endregion

		#region Private Static Variables

		private static readonly TouchLocation[] EmptyLocationArray = new TouchLocation[0];

		#endregion

		#region Public Constructor

		public TouchCollection(TouchLocation[] touches)
		{
			if (touches == null)
			{
				throw new ArgumentNullException("touches");
			}
			collection = touches;
		}

		#endregion

		#region Public Methods

		public bool FindById(int id, out TouchLocation touchLocation)
		{
			foreach (TouchLocation location in Collection)
			{
				if (location.Id == id)
				{
					touchLocation = location;
					return true;
				}
			}
			touchLocation = default(TouchLocation);
			return false;
		}

		#endregion

		#region Public IList<TouchLocation> Methods

		public int IndexOf(TouchLocation item)
		{
			for (int i = 0; i < Collection.Length; i += 1)
			{
				if (item == Collection[i])
				{
					return i;
				}
			}

			return -1;
		}

		public void Insert(int index, TouchLocation item)
		{
			Collection.Insert(index, item);
		}

		public void Add(TouchLocation item)
		{
			Collection.Add(item);
		}

		public void Clear()
		{
			Collection.Clear();
		}

		public bool Contains(TouchLocation item)
		{
			foreach (TouchLocation location in Collection)
			{
				if (item == location)
				{
					return true;
				}
			}

			return false;
		}

		public void CopyTo(TouchLocation[] array, int arrayIndex)
		{
			Collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(TouchLocation item)
		{
			return Collection.Remove(item);
		}

		public bool RemoveAt(int index)
		{
			return Collection.RemoveAt(index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		#endregion

		#region IEnumerator Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<TouchLocation> IEnumerable<TouchLocation>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		#endregion

		#region Enumerator

		// https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.touch.touchcollection.enumerator.aspx
		public struct Enumerator : IEnumerator<TouchLocation>
		{
			private readonly TouchCollection collection;
			private int position;

			internal Enumerator(TouchCollection collection)
			{
				this.collection = collection;
				position = -1;
			}

			public TouchLocation Current
			{
				get
				{
					return collection[position];
				}
			}

			public bool MoveNext()
			{
				position += 1;
				return (position < collection.Count);
			}

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get
				{
					return collection[position];
				}
			}

            void IEnumerator.Reset()
            {
                position = -1;
            }
		}

		#endregion
	}
}
