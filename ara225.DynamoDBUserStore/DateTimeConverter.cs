using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ara225.DynamoDBUserStore
{
	public class DateTimeConverter : IPropertyConverter
	{
		public DynamoDBEntry ToEntry(object value)
		{
			return ((DateTime)value).ToString("o");
		}

		public object FromEntry(DynamoDBEntry entry)
		{
			return DateTime.ParseExact(entry.AsString(), "o", null);
		}
	}
}
