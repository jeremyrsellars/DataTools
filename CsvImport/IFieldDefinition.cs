using System;

namespace CsvImport
{
   internal interface IFieldDefinition
   {
      string Name { get; set; }
      Type Type { get; set; }
      object Parse(string s);
   }

   internal class FieldDefinition : IFieldDefinition
   {
      public string Name { get; set; }
      public Type Type { get; set; }

      public virtual object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Convert.ChangeType(s, Type);
      }

      public static IFieldDefinition Create(Type type, string name = null)
      {
         var field = new FieldDefinition();
         if (type == typeof(Byte))
            field = new ByteFieldDefinition();
         if (type == typeof(Int16))
            field = new Int16FieldDefinition();
         if (type == typeof(Int32))
            field = new Int32FieldDefinition();
         if (type == typeof(Int64))
            field = new Int64FieldDefinition();
         if (type == typeof(DateTime))
            field = new DateTimeFieldDefinition();
         if (type == typeof(DateTimeOffset))
            field = new DateTimeOffsetFieldDefinition();
         if (type == typeof(Single))
            field = new SingleFieldDefinition();
         if (type == typeof(Double))
            field = new DoubleFieldDefinition();
         if (type == typeof(Boolean))
            field = new BooleanFieldDefinition();
         if (type == typeof(Guid))
            field = new GuidFieldDefinition();
         field.Type = type;
         field.Name = name;
         return field;
      }
   }

   class ByteFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Byte.Parse(s);
      }
   }

   class Int16FieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Int16.Parse(s);
      }
   }

   class Int32FieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Int32.Parse(s);
      }
   }

   class Int64FieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Int64.Parse(s);
      }
   }

   class DateTimeFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return DateTime.Parse(s);
      }
   }

   class DateTimeOffsetFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return DateTimeOffset.Parse(s);
      }
   }

   class SingleFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Single.Parse(s);
      }
   }

   class DoubleFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Double.Parse(s);
      }
   }

   class BooleanFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         if (s == "0" || s == Boolean.FalseString) return false;
         if (s == "1" || s == Boolean.TrueString) return true;
         return Boolean.Parse(s);
      }
   }

   class GuidFieldDefinition : FieldDefinition
   {
      public override object Parse(string s)
      {
         if (string.IsNullOrEmpty(s))
            return null;
         return Guid.Parse(s);
      }
   }
}