namespace Bb.Extended
{

    public class ModelDescriptor
    {

        public ModelDescriptor? Parent { get; internal set; }


        public Models? Root()
        {

            if (_root == null)
            {

                if (this.Parent == null)
                    return null;

                if (this.Parent is Models model)
                    _root = model;

                else
                    _root = this.Parent.Root();


            }

            return _root;

        }

        public int? Id { get; set; }

        private Models? _root;


    }


}
