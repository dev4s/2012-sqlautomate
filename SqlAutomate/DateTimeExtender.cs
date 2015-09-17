using System;

namespace SqlAutomate
{
	public static class DateTimeExtender
	{
		public static string ToNormalTime(this DateTime dateTime)
		{
			return dateTime.ToString("HH:mm:ss");
		}
	}
}