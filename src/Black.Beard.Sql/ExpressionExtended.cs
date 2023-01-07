using System.Linq.Expressions;
using System.Reflection;

namespace Bb
{
    public static class ExpressionExtended
    {

        public static PropertyInfo GetProperty(this Expression e)
        {

            var o = new PropertyVisitor();
            o.Visit(e);
            return o.Property;
        }

        private class PropertyVisitor : ExpressionVisitor
        {


            public PropertyVisitor()
            {

            }

            public PropertyInfo Property { get; private set; }

            protected override Expression VisitMember(MemberExpression node)
            {

                if (node.Member is PropertyInfo p)
                    Property = p;

                return base.VisitMember(node);
            }

        }


    }


}
