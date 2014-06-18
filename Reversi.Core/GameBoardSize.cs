using System;
using System.Diagnostics.Contracts;

namespace Reversi.Core
{
	using Algorithms;

	public sealed class GameBoardSize
	{
		#region 非表示メンバ

		private int _Width;
		private int _Height;

		internal bool _Contains (int x, int y)
		{
			return
				(0 <= x && x < _Width) &&
				(0 <= y && y < _Height);
		}

		#endregion

		public const int DefaultWidth = 8;
		public const int DefaultHeight = 8;
		public int Width
		{
			get
			{
				return _Width;
			}
		}
		public int Height
		{
			get
			{
				return _Height;
			}
		}
		public int Area
		{
			get
			{
				return _Width * _Height;
			}
		}
		public int ShorterSide
		{
			get
			{
				return Math.Min (_Width, _Height);
			}
		}
		public int LongerSide
		{
			get
			{
				return Math.Max (_Width, _Height);
			}
		}

		public GameBoardSize (int width, int height)
		{
			Contract.Assert (width >= 2);
			Contract.Assert (height >= 2);
			_Width = width;
			_Height = height;
		}
		public GameBoardSize () : this (DefaultWidth, DefaultHeight) { }
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
			return
				(other != null) &&
				(Width == other.Width && Height == other.Height);
		}
		public override int GetHashCode ()
		{
			return HashCombinator.Combine (Width.GetHashCode (), Height.GetHashCode ());
		}
	}
}
