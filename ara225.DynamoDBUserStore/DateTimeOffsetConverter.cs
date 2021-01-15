using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ara225.DynamoDBUserStore
{
	public class DateTimeOffsetConverter : IPropertyConverter
	{
		public DynamoDBEntry ToEntry(object value)
		{
			if (((DateTimeOffset?)value).HasValue)
			{
				return ((DateTimeOffset?)value).Value.ToString("o");
			}
			else
            {
				return null;
            }
		}

		public object FromEntry(DynamoDBEntry entry)
		{
			return (DateTimeOffset?)DateTimeOffset.ParseExact(entry.AsString(), "o", null);
		}
	}
}
