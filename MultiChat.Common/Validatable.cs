using System;

namespace MultiChat.Common
{
    public class Validatable<T>
    {
        public T Value { get; set; }
        public Predicate<T> Validator { get; set; }

        public Validatable(T value, Predicate<T> validator)
        {
            Value = value;
            Validator = validator;
        }

        public bool Valid()
        {
            return Validator(Value);
        } 
    }
}