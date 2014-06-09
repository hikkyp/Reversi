using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Core
{
	using Algorithms;

	public sealed class GameBoardSize
	{
		#region 非表示メンバ

		internal bool _Contains (int x, int y)
		{
			return
				0 <= x && x < Width &&
				0 <= y && y < Height;
		}

		#endregion

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Area { get { return Width * Height; } }
		public int ShorterSide { get { return Math.Min (Width, Height); } }
		public int LongerSide { get { return Math.Max (Width, Height); } }

		public GameBoardSize (int width, int height)
		{
			Contract.Assert (width >= 2);
			Contract.Assert (height >= 2);

			Width = width;
			Height = height;
		}
		public GameBoardSize () : this (8, 8) { }
		public bool Contains (GameBoardSpace boardSpace)
		{
			return _Contains (boardSpace.X, boardSpace.Y);
		}
		public override string ToString ()
		{
			return string.Format ("({0},{1})", Width, Height);
		}
		public override bool Equals (object obj)
		{
			var other = obj as GameBoardSize;
			if (other != null) {
				return Width == other.Width && Height == other.Height;
			}
			return false;
		}
		public override int GetHashCode ()
		{
			return HashCombinator.Combine (Width.GetHashCode (), Height.GetHashCode ());
		}
	}
}
