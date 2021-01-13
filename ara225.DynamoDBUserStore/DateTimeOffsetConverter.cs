using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ara225.DynamoDBUserStore
{
	public class DateTimeOffsetConverter : IPropertyConverter
	{
		public DynamoDBEntry ToEntry(object value)
		{
			return ((DateTimeOffset)value).ToString("o");
		}

		public object FromEntry(DynamoDBEntry entry)
		{
			return DateTimeOffset.ParseExact(entry.AsString(), "o", null);
		}
	}
}
