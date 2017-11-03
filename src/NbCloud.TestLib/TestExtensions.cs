using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbCloud.TestLib
{
    public static class TestExtensions
    {
        public static void ShouldThrows<T>(this Action action) where T : Exception
        {
            AssertHelper.ShouldThrows<T>(action);
        }

        public static void ShouldNull(this object value)
        {
            AssertHelper.WriteLineForShouldBeNull(value);
            Assert.IsNull(value);
        }

        public static void ShouldNotNull(this object value)
        {
            AssertHelper.WriteLineForShouldBeNotNull(value);
            Assert.IsNotNull(value);
        }

        public static void ShouldEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} equals {1}", value, expectedValue);
            Assert.AreEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
        }


        public static void ShouldSame(this object value, object expectedValue)
        {
            string message = string.Format("Should {2} same {0}:{1}", value.GetHashCode(), expectedValue.GetHashCode(), typeof(object).Name);
            Assert.AreSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
        }

        public static void ShouldNotSame(this object value, object expectedValue)
        {
            string message = string.Format("Should {2} not same {0}:{1}", value.GetHashCode(), expectedValue.GetHashCode(), typeof(object).Name);
            Assert.AreNotSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
        }

        public static void ShouldTrue(this bool result)
        {
            AssertHelper.WriteLineForShouldBeTrue(result);
            Assert.IsTrue(result);
        }

        public static void ShouldFalse(this bool result)
        {
            AssertHelper.WriteLineForShouldBeFalse(result);
            Assert.IsFalse(result);
        }

        public static void LogHashCode(this object value)
        {
            string message = string.Format("{0} <{1}>", value.GetHashCode(), value.GetType().Name);
            AssertHelper.WriteLine(message);
        }
        public static void LogHashCodeWiths(this object value, object value2)
        {
            string message = string.Format("{0} <{1}> {2} {3}<{4}>", value.GetHashCode(), value.GetType().Name, value == value2 ? "==" : "!=", value2.GetHashCode(), value2.GetType().Name);
            AssertHelper.WriteLine(message);
        }

        public static void Log(this object value)
        {
            if (value == null)
            {
                Debug.WriteLine("null");
            }

            var items = value as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    Debug.WriteLine(item);
                }
                return;
            }
            Debug.WriteLine(value);
        }

        public static string WithOkPrefix(this string value)
        {
            return AssertHelper.PrefixOk(value);
        }
        public static string WithKoPrefix(this string value)
        {
            return AssertHelper.PrefixKo(value);
        }
        public static string WithPrefix(this string value, bool isOk = true)
        {
            return AssertHelper.PrefixKo(value);
        }
    }
}