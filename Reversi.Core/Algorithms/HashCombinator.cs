namespace Reversi.Core.Algorithms
{
	public static class HashCombinator
	{
		public static int Combine (int value1, int value2)
		{
			return (value1 << 5) + value1 ^ value2;
		}
		public static int Combine (int value1, int value2, int value3)
		{
			return Combine (Combine (value1, value2), value3);
		}
		public static int Combine (int value1, int value2, int value3, int value4)
		{
			return Combine (Combine (Combine (value1, value2), value3), value4);
		}
		public static int Combine (int value1, int value2, int value3, int value4, int value5)
		{
			return Combine (Combine (Combine (Combine (value1, value2), value3), value4), value5);
		}
		public static int Combine (int value1, int value2, int value3, int value4, int value5, int value6)
		{
			return Combine (Combine (Combine (Combine (Combine (value1, value2), value3), value4), value5), value6);
		}
		public static int Combine (int value1, int value2, int value3, int value4, int value5, int value6, int value7)
		{
			return Combine (Combine (Combine (Combine (Combine (Combine (value1, value2), value3), value4), value5), value6), value7);
		}
		public static int Combine (int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8)
		{
			return Combine (Combine (Combine (Combine (Combine (Combine (Combine (value1, value2), value3), value4), value5), value6), value7), value8);
		}
		public static int Combine (int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, int value9)
		{
			return Combine (Combine (Combine (Combine (Combine (Combine (Combine (Combine (value1, value2), value3), value4), value5), value6), value7), value8), value9);
		}
		public static int Combine (object value1, object value2)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4, object value5)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode (), value5.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4, object value5, object value6)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode (), value5.GetHashCode (), value6.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4, object value5, object value6, object value7)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode (), value5.GetHashCode (), value6.GetHashCode (), value7.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4, object value5, object value6, object value7, object value8)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode (), value5.GetHashCode (), value6.GetHashCode (), value7.GetHashCode (), value8.GetHashCode ());
		}
		public static int Combine (object value1, object value2, object value3, object value4, object value5, object value6, object value7, object value8, object value9)
		{
			return Combine (value1.GetHashCode (), value2.GetHashCode (), value3.GetHashCode (), value4.GetHashCode (), value5.GetHashCode (), value6.GetHashCode (), value7.GetHashCode (), value8.GetHashCode (), value9.GetHashCode ());
		}
	}
}
