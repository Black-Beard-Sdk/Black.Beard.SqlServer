namespace Bb.SqlServer.Queries
{

    public class TableHint
    {

        protected TableHint(string hint)
        {
            this.Hint = hint;
        }

        public string Hint { get; }

        public override string ToString()
        {
            return Hint;
        }


        public static NoExpandTableHint NOEXPAND(params TableHintIndex[] indexs) { return new NoExpandTableHint(indexs); }

        public static TableHintIndex INDEX(params string[] indexValues) { return new TableHintIndex(indexValues); }

        public static ForseekTableHint FORCESEEK() { return new ForseekTableHint(); }
        public static ForseekTableHint FORCESEEK(string indexValue, params string[] indexColumnNames)
        {
            return new ForseekTableHint(indexValue, indexColumnNames);
        }

        public static SpatialWindowMaxCellsTableHint SPATIAL_WINDOW_MAX_CELLS(int indexValue)
        {
            return new SpatialWindowMaxCellsTableHint(indexValue);
        }
        public static TableHint FORCESCAN = new TableHint("FORCESCAN");
        public static TableHint READUNCOMMITTED = new TableHint("READUNCOMMITTED");

        public static class Limited
        {
            public static TableHintLimited KEEPIDENTITY = new TableHintLimited("KEEPIDENTITY");
            public static TableHintLimited KEEPDEFAULTS = new TableHintLimited("KEEPDEFAULTS");
            public static TableHintLimited HOLDLOCK = new TableHintLimited("HOLDLOCK");
            public static TableHintLimited IGNORE_CONSTRAINTS = new TableHintLimited("IGNORE_CONSTRAINTS");
            public static TableHintLimited IGNORE_TRIGGERS = new TableHintLimited("IGNORE_TRIGGERS");
            public static TableHintLimited NOLOCK = new TableHintLimited("NOLOCK");
            public static TableHintLimited NOWAIT = new TableHintLimited("NOWAIT");
            public static TableHintLimited PAGLOCK = new TableHintLimited("PAGLOCK");
            public static TableHintLimited READCOMMITTED = new TableHintLimited("READCOMMITTED");
            public static TableHintLimited READCOMMITTEDLOCK = new TableHintLimited("READCOMMITTEDLOCK");
            public static TableHintLimited READPAST = new TableHintLimited("READPAST");
            public static TableHintLimited REPEATABLEREAD = new TableHintLimited("REPEATABLEREAD");
            public static TableHintLimited ROWLOCK = new TableHintLimited("ROWLOCK");
            public static TableHintLimited SERIALIZABLE = new TableHintLimited("SERIALIZABLE");
            public static TableHintLimited SNAPSHOT = new TableHintLimited("SNAPSHOT");
            public static TableHintLimited TABLOCK = new TableHintLimited("TABLOCK");
            public static TableHintLimited TABLOCKX = new TableHintLimited("TABLOCKX");
            public static TableHintLimited UPDLOCK = new TableHintLimited("UPDLOCK");
            public static TableHintLimited XLOCK = new TableHintLimited("XLOCK");
        }

    }

}
