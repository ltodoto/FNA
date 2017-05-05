#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2017 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
#endregion

namespace Microsoft.Xna.Framework
{
	internal static class TitleLocation
	{
		#region Public Static Properties

		public static string Path
		{
			get;
			private set;
		}

		#endregion

		#region Static Constructor

		static TitleLocation()
		{
			Path = AppDomain.CurrentDomain.BaseDirectory;
			string altTitleDir = Environment.GetEnvironmentVariable("FNA_TITLEDIR");
			if (!String.IsNullOrEmpty(altTitleDir))
			{
				Path = altTitleDir;
			}
		}

		#endregion
	}
}

