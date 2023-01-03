using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System.Diagnostics;
using System.Reflection;

namespace Black.Beard.SqlServer.Tests
{
    [TestClass]
    public class MergeUnitTest
    {

        public MergeUnitTest()
        {
            this._root = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

        [TestMethod]
        public void Test1()
        {

            var m = new Merge()

                .Top()

                .Into("targetSchema", "targetTable", c =>
                {
                    c.As("t")
                     .Hints
                        .Add(TableHint.Limited.NOLOCK)
                    ;
                })

                .Using("sourceSchema", "sourceTable", c =>
                {
                    c.As("s")
                    ;
                })

                .On(SqlPredicateExpr.Reference("s", "id").Equal(SqlExpr.Reference("t","id")))

                .WhenMatched(c =>
                {
                    c.Update(new SetColumnClause("col1", SqlExpr.Constant("ValueCol1")))
                    ;
                })

                .WhenNotMatchedByTarget(c =>
                {
                    c.Insert(new SetColumnClause( "col1",  SqlExpr.Constant("ValueCol1")))
                    ;
                })

                .WhenNotMatchedBySource(c =>
                {
                    c.Delete()
                    ;
                })

                ;

            var o = m.ToString();

            Assert.AreEqual(o, @"
MERGE
	INTO [targetSchema].[targetTable] WITH (NOLOCK)
	
	USING [sourceSchema].[sourceTable]
	
	ON [s].[id] = [t].[id]
	
	WHEN MATCHED THEN UPDATE SET
		[col1] = ValueCol1
	
	WHEN NOT MATCHED BY TARGET THEN INSERT
	(
		[col1]
	)
	VALUES
	(
		ValueCol1
	)
	
	WHEN NOT MATCHED BY SOURCE THEN DELETE
	
;
");

        }


        private readonly string _root;


    }
}