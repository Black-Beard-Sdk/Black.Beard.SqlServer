namespace Bb.Extended
{


    public class DiagramDiskTableModel : ModelDescriptor
    {

        public DiagramDiskTableModel()
        {
            Position = new Position();
        }

        public int ApplicationEnvironmentId { get; set; }
        
        public int DiskTableId { get; set; }

        public Position Position { get; set; }


    }



}
