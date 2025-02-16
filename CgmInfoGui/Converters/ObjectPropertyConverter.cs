using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;
using CgmInfoGui.Controls.PropertyGrid;

namespace CgmInfoGui.Converters;

public sealed class ObjectPropertyConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [object selectedObject, ReadOnlyPropertyGrid propertyGrid])
            return null;

        var newPropertyMetadata = MakeProperties(selectedObject);
        return BuildSource(newPropertyMetadata)
            .AddMainColumn("Name", s => s.Name, s => s.ChildProperties)
            .AddColumn("Value", s => s.Value)
            .Finalize();
    }

    private static List<MetadataPropertyValue> MakeProperties(object selectedObject)
    {
        var newPropertyMetadata = new List<MetadataPropertyValue>();
        if (selectedObject is string)
            return newPropertyMetadata;

        var metadataAttrib = selectedObject.GetType().GetCustomAttribute<MetadataTypeAttribute>();
        var properties = metadataAttrib != null
            ? TypeDescriptor.GetProperties(metadataAttrib.MetadataClassType)
            : TypeDescriptor.GetProperties(selectedObject);

        foreach (PropertyDescriptor? property in properties)
        {
            if (property == null)
                continue;
            if (!property.IsBrowsable)
                continue;

            newPropertyMetadata.Add(MakeProperty(selectedObject, property));
        }

        return newPropertyMetadata;
    }

    private static MetadataPropertyValue MakeProperty(object selectedObject, PropertyDescriptor property)
    {
        object? propertyValue = property.GetValue(selectedObject);
        List<MetadataPropertyValue> childProperties = [];
        if (property.ComponentType.GetCustomAttribute<TypeConverterAttribute>() is { } typeConverterAttribute)
        {
            var typeConverterType = Type.GetType(typeConverterAttribute.ConverterTypeName);
            if (typeConverterType != null && propertyValue is not null)
                propertyValue = ((TypeConverter?)Activator.CreateInstance(typeConverterType))?.ConvertFrom(propertyValue);
        }
        if (propertyValue is IEnumerable enumerable and not string)
        {
            const int maxItems = 100;
            var enumerableT = enumerable.OfType<object>();
            if (!enumerableT.TryGetNonEnumeratedCount(out int collectionCount))
            {
                enumerableT = enumerableT.ToArray();
                collectionCount = enumerableT.Count();
            }
            childProperties.Add(new("(Count)", collectionCount <= maxItems ? collectionCount.ToString() : $"{maxItems} of {collectionCount}", []));

            // TODO: especially for large arrays, showing all items is not something we should be doing. but is this limit good enough?
            childProperties.AddRange(enumerableT.Take(maxItems).Select((o, i) => new MetadataPropertyValue($"[{i}]", Stringify(o), MakeProperties(o))));
        }
        else if (propertyValue?.GetType() is { IsValueType: false } && propertyValue is not string)
        {
            childProperties.AddRange(MakeProperties(propertyValue));
        }

        return new(property.DisplayName, Stringify(propertyValue), childProperties);
    }

    private static string Stringify(object? o) => o switch
    {
        byte b => $"{b} (0x{b:X2})",
        sbyte b => $"{b} (0x{b:X2})",
        ushort s => $"{s} (0x{s:X4})",
        short s => $"{s} (0x{s:X4})",
        uint i => $"{i} (0x{i:X8})",
        int i => $"{i} (0x{i:X8})",
        ulong l => $"{l} (0x{l:X16})",
        long l => $"{l} (0x{l:X16})",
        _ => System.Convert.ToString(o) ?? string.Empty,
    };
    private static SourceBuilder<T> BuildSource<T>(IEnumerable<T> source)
        where T : class
        => new(source);

    private sealed class SourceBuilder<TModel> where TModel : class
    {
        private readonly HierarchicalTreeDataGridSource<TModel> _source;
        public SourceBuilder(IEnumerable<TModel> source) => _source = new(source);
        public SourceBuilder<TModel> AddMainColumn<TValue>(string header, Expression<Func<TModel, TValue?>> expression, Func<TModel, IEnumerable<TModel>> childSelector, GridLength? width = null, TextColumnOptions<TModel>? options = null)
        {
            _source.Columns.Add(new HierarchicalExpanderColumn<TModel>(
                new TextColumn<TModel, TValue>(header, expression, width, options),
                childSelector));
            return this;
        }
        public SourceBuilder<TModel> AddColumn<TValue>(string header, Expression<Func<TModel, TValue?>> expression, GridLength? width = null, TextColumnOptions<TModel>? options = null)
        {
            _source.Columns.Add(new TextColumn<TModel, TValue>(header, expression, width, options));
            return this;
        }
        public HierarchicalTreeDataGridSource<TModel> Finalize() => _source;
    }
    public record class MetadataPropertyValue(string Name, string Value, List<MetadataPropertyValue> ChildProperties);
}

