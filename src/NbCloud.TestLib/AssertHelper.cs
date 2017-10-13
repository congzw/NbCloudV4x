using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbCloud.TestLib
{
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