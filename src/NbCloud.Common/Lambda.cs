using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NbCloud.Common
{
    //borrowed from https://github.com/jagregory/lambdanator

    public class λ
    {
        public static MemberInfo Reflect(Expression<Func<object>> expression)
        {
            return Reflect(expression.Body);
        }
        
        public static MemberInfo Reflect<T>(Expression<Func<T, object>> expression)
        {
            return Reflect(expression.Body);
        }

        public static MemberInfo Reflect(Expression<Action> expression)
        {
            return Reflect(expression.Body);
        }

        public static MemberInfo Reflect<T>(Expression<Action<T>> expression)
        {
            return Reflect(expression.Body);
        }

        public static MemberInfo Reflect(Expression expression)
        {
            var memberAccess = expression as MemberExpression;

            if (memberAccess != null)
                return memberAccess.Member;

            var unary = expression as UnaryExpression;

            if (unary != null)
                return Reflect(unary.Operand);

            var call = expression as MethodCallExpression;

            if (call != null)
                return call.Method;

            throw new ArgumentException("Not a member access", "expression");
        }
    }

    public class Lambda : λ
    {}
}