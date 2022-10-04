using AutoMapper;
using System;

namespace ESA.Core.Utils
{
    public class DateTimeOffsetConverter : ITypeConverter<DateTimeOffset, DateTime>
    {
        public DateTime Convert(DateTimeOffset source, DateTime destination, ResolutionContext context)
        {
            return source.DateTime;
        }
    }

    public class ToDateTimeOffsetConverter : ITypeConverter<DateTime, DateTimeOffset>
    {
        public DateTimeOffset Convert(DateTime source, DateTimeOffset destination, ResolutionContext context)
        {
            return destination;
        }
    }

    public class ToDateOnlyConverter : ITypeConverter<DateTime, DateOnly>
    {
        public DateOnly Convert(DateTime source, DateOnly destination, ResolutionContext context)
        {
            return DateOnly.FromDateTime(source);
        }
    }
}
