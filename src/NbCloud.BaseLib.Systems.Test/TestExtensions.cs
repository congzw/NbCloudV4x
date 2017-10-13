using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbCloud.BaseLib.Systems
{
    public static class TestExtensions
    {
        public static void ShouldThrows<T>(this Action action) where T : Exception
        {
            AssertHelper.ShouldThrows<T>(action);
        }

        public static void ShouldNull(this object value)
        {
            Assert.IsNull(value);
        }

        public static void ShouldNotNull(this object value)
        {
            Assert.IsNotNull(value);
        }

        public static void ShouldEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} equals {1}", value, expectedValue);
            Assert.AreEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
        }

        public static void ShouldTrue(this bool result)
        {
            Assert.IsTrue(result);
        }

        public static void ShouldFalse(this bool result)
        {
            Assert.IsFalse(result);
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
    }

    public class AssertHelper
    {
        public static readonly string OkMark = "✔";
        public static readonly string KoMark = "✘";

        public static void ShouldThrows<T>(Action action) where T : Exception
        {
            T expectedEx = null;
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var baseException = ex.GetBaseException();
                    WriteLineOk("抛出了异常:" + baseException.Message);
                    expectedEx = baseException as T;
                }
                else
                {
                    WriteLineOk("抛出了异常:" + ex.Message);
                    expectedEx = ex as T;
                }
            }
            Assert.IsNotNull(expectedEx, PrefixKo("没有发现应该抛出的异常: " + typeof(T).Name));
        }
        public static string PrefixOk(string value)
        {
            return OkMark + " " + value;
        }
        public static string PrefixKo(string value)
        {
            return KoMark + " " + value;
        }
        public static void WriteLineOk(string message)
        {
            Debug.WriteLine(PrefixOk(message));
        }
        public static void WriteLineKo(string message)
        {
            Debug.WriteLine(PrefixKo(message));
        }
    }
}