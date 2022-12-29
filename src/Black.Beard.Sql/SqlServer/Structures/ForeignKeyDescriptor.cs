using Microsoft.VisualBasic;

namespace Bb.SqlServer.Structures
{
    public class ForeignKeyDescriptor : SqlServerDescriptor
    {


        public ForeignKeyDescriptor()
        {
            LocalColumns = new ColumnReferenceListDescriptor();
            RemoteColumns = new RemoteColumnReferenceListDescriptor();
        }


        public ColumnReferenceListDescriptor LocalColumns { get; set; }
                
        public RemoteColumnReferenceListDescriptor RemoteColumns { get; set; }

        public ForeignKeyDescriptor AddRemoteColumns(params string[] columns)
        {

            foreach (var item in columns)
                RemoteColumns.Add(new ColumnReferenceDescriptor() { Name = item });

            return this;

        }

        public ForeignKeyDescriptor AddLocalColumns(params string[] columns)
        {

            foreach (var item in columns)
                LocalColumns.Add(new ColumnReferenceDescriptor() { Name = item });

            return this;

        }

        public ForeignKeyDescriptor DeleteCascade(bool value)
        {
            OnDeleteCascade = value;
            return this;
        }

        public ForeignKeyDescriptor UpdateCascade(bool value)
        {
            OnUpdateCascade = value;
            return this;
        }

        public bool IsDifferent(ForeignKeyDescriptor target)
        {

            if (this.Name != target.Name)
                return true;

            if (this.LocalColumns.Count != target.LocalColumns.Count )
                return true;

            if (this.RemoteColumns.Count != target.RemoteColumns.Count)
                return true;

            for (int i = 0; i < this.LocalColumns.Count; i++)
                if (this.LocalColumns[i].IsDifferent(target.LocalColumns[i]))
                    return true;

            for (int i = 0; i < this.RemoteColumns.Count; i++)
                if (this.RemoteColumns[i].IsDifferent(target.RemoteColumns[i]))
                    return true;

            if (this.OnDeleteCascade != target.OnDeleteCascade)
                return true;

            if (this.OnUpdateCascade!= target.OnUpdateCascade)
                return true;

            return false;

        }

        public bool OnDeleteCascade { get; set; } = false;

        public bool OnUpdateCascade { get; set; } = false;


    }


}