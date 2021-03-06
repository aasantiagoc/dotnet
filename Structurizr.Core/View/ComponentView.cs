using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// A system context view.
    /// </summary>
    [DataContract]
    public class ComponentView : StaticView
    {

        public override string Name
        {
            get
            {
                return SoftwareSystem.Name + " - " + Container.Name + " - Components";
            }
        }

        public Container Container { get; set; }

        private string containerId;

        /// <summary>
        /// The ID of the container this view is associated with.
        /// </summary>
        [DataMember(Name="containerId", EmitDefaultValue=false)]
        public string ContainerId {
            get
            {
                if (Container != null)
                {
                    return Container.Id;
                } else
                {
                    return containerId;
                }
            }
            set
            {
                this.containerId = value;
            }
        }

        internal ComponentView() : base()
        {
        }

        internal ComponentView(Container container, string key, string description) : base(container.SoftwareSystem,key,  description)
        {
            this.Container = container;
        }

        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
            AddAllContainers();
            AddAllComponents();
        }

        public override void Add(SoftwareSystem softwareSystem)
        {
            if (softwareSystem != null && !softwareSystem.Equals(SoftwareSystem))
            {
                AddElement(softwareSystem, true);
            }
        }

        public void AddAllContainers()
        {
            foreach (Container container in SoftwareSystem.Containers)
            {
                Add(container);
            }
        }

        public void Add(Container container)
        {
            if (container != null && !container.Equals(Container))
            {
                AddElement(container, true);
            }
        }

        public void Remove(Container container)
        {
            RemoveElement(container);
        }

        public void AddAllComponents()
        {
            foreach (Component component in Container.Components)
            {
                Add(component);
            }
        }

        public void Add(Component component)
        {
            AddElement(component, true);
        }

        public void Remove(Component component)
        {
            RemoveElement(component);
        }

    }
}
