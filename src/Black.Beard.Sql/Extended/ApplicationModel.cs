namespace Bb.Extended
{


    public class ApplicationModel : ListNamedModelDescriptor<EnvironmentModel>
    {


        public ApplicationModel() 
        {
        
        
        }

        public ApplicationModel AddEnvironment(string environmentName, Action<EnvironmentModel>? action = null)
        {

            var application = new EnvironmentModel() 
            {
                Name = environmentName
            };

            this.Add(application);

            if (action != null)
                action(application);

            return this;

        }


    }


}
